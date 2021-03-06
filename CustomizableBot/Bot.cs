﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Threading;

namespace CustomizableBot
{
    [Serializable]
    class Bot
    {
        System.Threading.Timer timer;
        protected ControlForm form;

        List<int[]> inputPoints = new List<int[]>();
        //Botのパラメータ保存用のクラス
        [Serializable]class BotData
        {
            public InputRange inputRange;
            public List<int[]> inputPoints;
            //...etc
            public BotData(InputRange inputRange, List<int[]> inputPoints)
            {
                this.inputRange = inputRange;
                this.inputPoints = inputPoints;
            }
        }
        protected Bot(){}
        public Bot(ControlForm form)
        {
            this.form = form;
        }
        void initialize()
        {


        }

        internal void start()
        {
            //BOTの設定の初期化
            initialize();

            //時間計測用ストップウォッチ
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();


            timer = new System.Threading.Timer((state) => {

                //情報の取得
                //Point poi=UIControl.getCursorPos();//カーソル位置
                Bitmap bmp = getInput();//選択した画面
                List<Color> data = getInputPointsColor();//選択したn番目の点の色 data[n]
                var brightness = getInputPointsBrightness();//選択したn番目の点の明るさ(HSB)
                                                            //var pos = inputPoints;//選択したn番目の点の位置 x:inputPoints[n][0], y:inputPoints[n][1]


                //アクティブな画面への入力
                //UIControl.sendKeys("{UP}");
                //UIControl.SetCursorPos(0,0);
                //UIControl.click();
                //UIControl.keyDown(0x50, 10);//{P}を10回入力
                //UIControl.keyDown(0x28, 10); //{↓}を押下
                //UIControl.keyUp(0x28,10);
                //UIControl.keyDown(0x26, 10); //{↑}を押下
                Thread thread = new Thread(() =>
                {
                    UIControl.keyPress(0x28, 10);//↓を10回入力
                });
                thread.Start();

                //ログの出力方法
                form.logWithDispose(bmp);//画像の表示
                form.log(sw.Elapsed+"");//文字列の表示

            }, null, 0, 400);
        }

        internal void stop()
        {
            timer.Dispose();
        }

        internal void restart()
        {
            stop();
            start();
        }
        private Bitmap getInput()
        {
            Bitmap bmp;
            if (inputRange.left == inputRange.right || inputRange.up == inputRange.down)
            {
                bmp = UIControl.captureScreen();
            }
            else bmp = UIControl.trimedCaptureScreen(inputRange.left, inputRange.up, inputRange.right, inputRange.down);
            return bmp;
        }

        private List<Color> getInputPointsColor()
        {
            Bitmap bmp;
            bmp = UIControl.captureScreen();
           
            List<Color> col = new List<Color>();
            foreach(var pos in inputPoints)
            {
                col.Add(bmp.GetPixel(pos[0],pos[1]));
            }
            bmp.Dispose();
            
            return col;
        }

        private List<float> getInputPointsBrightness()
        {
            Bitmap bmp;
            bmp = UIControl.captureScreen();

            List<float> col = new List<float>();
            foreach (var pos in inputPoints)
            {
                col.Add(bmp.GetPixel(pos[0], pos[1]).GetBrightness());
            }
            bmp.Dispose();

            return col;
        }

        #region botへの入力範囲の設定
        InputRange inputRange;

        internal int inputGranularity=30;
        [Serializable]
        public struct InputRange
        {
            internal int left;//start
            internal int up;
            internal int right;//end
            internal int down;
            public InputRange(int left,int up,int right,int down)
            {
                this.left = left;
                this.up = up;
                this.right = right;
                this.down = down;
            }
        }


        internal void setTrimRange(int left, int up, int right, int down)
        {
            inputRange = new InputRange(left, up, right, down);
            for (int y = inputRange.up; y < inputRange.down; y+= inputGranularity)
            {
                for (int x = inputRange.left; x < inputRange.right; x+= inputGranularity)
                {
                    inputPoints.Add(new int[] {x,y});
                }
            }
            showInputRange();
        }

        private void showInputRange()
        {
            Bitmap bmp = UIControl.captureScreen();


            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(newBmp);
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            ia.SetGamma((float)0.3);
            if (inputRange.left == inputRange.right && inputRange.up == inputRange.down)
            {
                g.DrawImage(bmp, new Point(0, 0));
            }
            else
            {
                g.DrawImage(bmp,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
            }


            for (int y = inputRange.up; y < inputRange.down; y++)
            {
                for (int x = inputRange.left; x < inputRange.right; x++)
                {
                    newBmp.SetPixel(x, y, bmp.GetPixel(x, y));
                }
            }
            foreach (var pos in inputPoints)
            {
                g.FillEllipse(Brushes.Blue, new Rectangle(pos[0] - 8, pos[1] - 8, 16, 16));
            }
            g.Dispose();

            form.logWithDispose(newBmp);
            bmp.Dispose();
        }

        #endregion

        #region botへの入力点の設定
        public void addInputPoints(int x,int y)
        {
            inputPoints.Add(new int[] {x,y});

            //描画
            showInputRange();
        }
        internal void deleteInputPoints()
        {
            inputPoints = new List<int[]>();

            //描画
            showInputRange();
        }
        #endregion

        internal void save()
        {
            BotData data = new BotData(inputRange,inputPoints);
            FileControl.save(data);
        }

        internal void load()
        {
            Object obj = FileControl.load();
            if (obj == null) return;
            var loadBotData = (BotData)obj;
            this.inputRange = loadBotData.inputRange;
            this.inputPoints = loadBotData.inputPoints;
            showInputRange();
        }
    }

    static class UIControl
    {
        #region 画面の取得
        private const int SRCCOPY = 13369376;
        private const int CAPTUREBLT = 1073741824;
        [DllImport("user32.dll")]private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        private static extern int BitBlt(IntPtr hDestDC,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hSrcDC,
            int xSrc,
            int ySrc,
            int dwRop);

        [DllImport("user32.dll")]
        private static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

        public static Bitmap captureScreen()
        {
            IntPtr disDC = GetDC(IntPtr.Zero);
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr hDC = g.GetHdc();
            BitBlt(hDC, 0, 0, bmp.Width, bmp.Height,
                disDC, 0, 0, SRCCOPY);

            //解放の処理
            g.ReleaseHdc(hDC);
            g.Dispose();
            ReleaseDC(IntPtr.Zero, disDC);

            return bmp;
        }
        public static Bitmap trimedCaptureScreen(int left, int up, int right, int down)
        {
            Bitmap bmp = captureScreen();
            Rectangle rect = new Rectangle(left, up, right-left, down-up);
            Bitmap trimedBmp = bmp.Clone(rect, bmp.PixelFormat);
            bmp.Dispose();
            return trimedBmp;
        }

        //RGBの取得
        public static Color[,] getPixels()
        {
            var bmp = captureScreen();
            var size = bmp.Size;
            Color[,] res = new Color[size.Width, size.Height];
            for (int y = 0; y < size.Height; y++) { 
                for (int x = 0; x < size.Width; x++)
                {
                    res[x, y] = bmp.GetPixel(x, y);
                }
            }
            return res;
        }
        //明るさの取得
        public static float[,] getBrightness()
        {
            var bmp = captureScreen();
            var size = bmp.Size;
            float[,] res = new float[size.Width, size.Height];
            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    res[x, y] = bmp.GetPixel(x, y).GetBrightness();
                }
            }
            return res;
        }

        #endregion
        #region カーソル位置の取得
        //SendKeys
        internal static Point getCursorPos()
        {
            return Cursor.Position;
        }
        #endregion
        #region キーストロークの送信
        internal static void sendKeys(String key)
        {
            SendKeys.SendWait(key);
        }

        [DllImport("user32.dll")]
        private extern static void SendInput(
int nInputs, ref INPUT pInputs, int cbsize);

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };
        private const int INPUT_MOUSE = 0;
        private const int INPUT_KEYBOARD = 1;
        private const int INPUT_HARDWARE = 2;
        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int KEYEVENTF_KEYUP = 0x2;

        [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
        private extern static int MapVirtualKey(
        int wCode, int wMapType);
        internal static void keyDown(short i,int num)
        {
            INPUT[] input = new INPUT[num];
            for (int n = 0; n < num; n++)
            {
                input[n].type = INPUT_KEYBOARD;
                input[n].ki.wVk = i;
                input[n].ki.wScan = (short)MapVirtualKey(input[0].ki.wVk, 0);
                input[n].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN;
                input[n].ki.dwExtraInfo = 0;
                input[n].ki.time = 0;
            }

            SendInput(num, ref input[0], Marshal.SizeOf(input[0]));
        }
        internal static void keyUp(short i, int num)
        {
            INPUT[] input = new INPUT[num];
            for (int n = 0; n < num; n++)
            {
                input[n].type = INPUT_KEYBOARD;
                input[n].ki.wVk = i;
                input[n].ki.wScan = (short)MapVirtualKey(input[0].ki.wVk, 0);
                input[n].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
                input[n].ki.dwExtraInfo = 0;
                input[n].ki.time = 0;
            }

            SendInput(num, ref input[0], Marshal.SizeOf(input[0]));
        }

        internal static void keyPress(short i, int num)
        {
            INPUT[] input = new INPUT[num*2];
            for (int n = 0; n < num*2; n=n+2)
            {
                input[n].type = INPUT_KEYBOARD;
                input[n].ki.wVk = i;
                input[n].ki.wScan = (short)MapVirtualKey(input[0].ki.wVk, 0);
                input[n].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN;
                input[n].ki.dwExtraInfo = 0;
                input[n].ki.time = 0;

                input[n+1].type = INPUT_KEYBOARD;
                input[n+1].ki.wVk = i;
                input[n+1].ki.wScan = (short)MapVirtualKey(input[0].ki.wVk, 0);
                input[n+1].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
                input[n+1].ki.dwExtraInfo = 0;
                input[n+1].ki.time = 0;
            }

            SendInput(num*2, ref input[0], Marshal.SizeOf(input[0]));
        }
        #endregion
        #region マウス入力
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetCursorPos(int X, int Y);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        internal static void click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        #endregion
    }

    static class FileControl
    {
        internal static void save(Object saveData)
        {
            System.IO.Directory.CreateDirectory(@"Userdata");
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Directory.GetCurrentDirectory() + @"\Userdata";
            sfd.FileName = "dat" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = sfd.OpenFile();
                BinaryFormatter bF = new BinaryFormatter();
                bF.Serialize(fileStream, saveData);
                fileStream.Close();
            }
        }

        internal static Object load()
        {
            Object loadedData = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + @"\Userdata";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream fileStream = ofd.OpenFile();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                loadedData = binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
            return loadedData;
        }
    }
}