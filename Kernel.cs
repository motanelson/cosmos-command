﻿using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Point = Cosmos.System.Graphics.Point;
using Sys = Cosmos.System;
using Cosmos.HAL;
using Cosmos.Core.IOGroup;

namespace CosmosKernel3
{

    public class windowss
    {
        public int x = 0;
        public int y = 0;
        public int w = 0;
        public int h = 0;
        public Bitmap dc;
        public int colorss;
    }
    public class mmouse
    {
        public int xx = 0;
        public int yy = 0;
        public int ini = 0;
        public int clicks =0;
    }
    public class Kernel : Sys.Kernel
    {
        Canvas canvas;
        int maxy;
        int maxx;
        int maxwins;
        int cursorSize;
        int xxxx=0;
        int yyyy=0;
        Color colorCursor;
        Color desktopColor;
        windowss cursors;
        Boolean Mmouseini=false;

        int parts(int i,int t)
        {
            return i/t;
        }
        void getCursor(Bitmap bts,int x, int y)
        {
            int n=0;
            int nn = 0;
            for (n = 0; n < cursorSize; n++)
            {
                for (nn = 0; nn < cursorSize; nn++)
                {
                    psets(bts, nn, n, canvas.GetPointColor(x+nn, y+n).ToArgb());
                }
            }
        }

        void drawWindows(windowss[] wins)
        {
            int n = 0;
            canvas.Clear(desktopColor);
            for (n = 0; n < wins.Length; n++)
            {
                canvas.DrawImage(wins[n].dc, new Point(wins[n].x, wins[n].y));
            }
            canvas.Display();
        }
        windowss createWindow(int x,int y,int w,int h,int colorss)
        {
            windowss windowsss = new windowss();
            windowsss.y = y;
            windowsss.x = x;
            windowsss.w = w;
            windowsss.h = h;
            windowsss.dc = createsbitmap((uint)windowsss.w,(uint) windowsss.h);
            windowsss.colorss = colorss;
            fills(windowsss.dc, windowsss.colorss);
            rets(windowsss.dc, 0, 0, (int)w - 1, (int)w - 1, 0);
            return windowsss;
        }
        void rets(Bitmap b, int x, int y, int x1, int y1, int colors)
        {

            hlines(b, x, y, x1, colors);
            hlines(b, x, y1, x1, colors);
            vlines(b, x, y, y1, colors);
            vlines(b, x1, y, y1, colors);


        }
        void boxs(Bitmap b, int x, int y,int x1, int y1, int colors)
        {
            int n = 0;
    

            if (y1>=y) {
                for (n = 0; n < y1 - y; n++)
                {
                    hlines(b,x, y + n, x1, colors);  
                }
            }
        }
        void vlines(Bitmap b, int x, int y, int y1, int colors)
        {
            int n = 0;
            int[] bt = b.rawData;
            if (x < b.Width && y < b.Height && x > -1 && y > -1 && y1 < b.Height && y1 > -1 && y1 >= y)
            {
                for (n = 0; n < y1 - y; n++)
                {
                    bt[y * b.Width + x + (n* b.Width)] = colors;
                }
            }
        }
        void hlines(Bitmap b, int x, int y,int x1, int colors)
        {
            int n = 0;
            int[] bt = b.rawData;
            if (x < b.Width && y < b.Height && x > -1 && y > -1 && x1 < b.Width && x1 > -1 && x1 >= x) {
                for (n = 0; n < x1 - x;n++)
                {
                    bt[y * b.Width + x+n] = colors;
                }
            }
        }
        void psets(Bitmap b,int x , int y,int colors)
        {
            int n = 0;
            int[] bt = b.rawData;
            if (x< b.Width && y<b.Height && x>-1 && y>-1) bt[y*b.Width+x] = colors;
        }
        int colors(byte reds,byte greens ,byte blues) {
            return blues | greens << 8 | reds  <<16;
        }
        Bitmap createsbitmap(uint x, uint y)
        {
            Bitmap bitmap = new Bitmap(x, y, ColorDepth.ColorDepth32);
            return bitmap;
        }
        void fills(Bitmap b,int colors)
        {
            int n = 0;
            int[] bt = b.rawData;
            for (n = 0; n < b.Height * b.Width; n++) bt[n] = colors;
        }
        mmouse drawcursor()
        {
            Pen pen = new Pen(Color.DarkGreen);
            int n = 0;
            int x = 0;
            int y = 0;
            int xx = maxx - 1;
            int yy = maxy - 1;
            Boolean c1 = false;
            mmouse Mmouse=new mmouse();
            int c = new Pen(Color.Black).ValueARGB;
            if (!Mmouseini)
            {
                Mmouseini = true;
                cursors = createWindow(0, 0, cursorSize, cursorSize, colors(0, (byte)parts(0xff, n), 0));

                x = (int)Sys.MouseManager.X;
                n = 0;
                y = (int)Sys.MouseManager.Y;
                xxxx = x;
                yyyy = y;
            }
            xx = xxxx;
            yy = yyyy;
                while (n==0)
            {
                x = (int)Sys.MouseManager.X;
                n = (int)Sys.MouseManager.MouseState;
                y = (int)Sys.MouseManager.Y;
                if (x != xx || y != yy)
                {
                    if (!c1)
                    {
                        xx = x;
                        yy = y;
                        getCursor(cursors.dc, x, y);
                        c1 = true;
                    }
                    else
                    {

                        canvas.DrawImage(cursors.dc, new Point(xx, yy));
                        getCursor(cursors.dc, x, y);
                        canvas.DrawFilledEllipse(new Pen(colorCursor), new Point(x + (cursorSize / 2), y + (cursorSize / 2)), (cursorSize - 1) / 2, (cursorSize - 1) / 2);
                        xx = x;
                        yy = y;

                        canvas.Display();
                        
                    }
                }

            }
            Mmouse.xx = x;
            Mmouse.yy = y;
            Mmouse.clicks = n;
            return Mmouse;
        }
        protected override void BeforeRun()
        {
            maxx = 640;
            maxy = 480;
            maxwins = 10;
            cursorSize = 8;
            colorCursor=Color.White ;
            desktopColor = Color.Green;
            Console.WriteLine("start.");
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(maxx, maxy, ColorDepth.ColorDepth32));
            canvas.Clear(Color.Green);
            Sys.MouseManager.ScreenHeight =(uint) (maxy-cursorSize)  ;
            Sys.MouseManager.ScreenWidth =(uint) (maxx - cursorSize) ;

        }

        protected override void Run()
        {
            Pen pen = new Pen(Color.DarkGreen);
            int n = 0;
            int x = 0;
            int y = 0;
            int xx = maxx-1;
            int yy = maxy-1;
            mmouse Mmouse=new mmouse();
            windowss cursors= createWindow(0, 0, cursorSize, cursorSize, colors(0, (byte)parts(0xff, n), 0));
            Boolean  c1 = false;
           int c = new Pen(Color.Black).ValueARGB ;
            windowss[] windowsss = new windowss[maxwins];
        
            for (n=0;n< maxwins; n++) windowsss[n]=createWindow(n * 10 + 8, n * 10 + 8,100, 100,colors(0,(byte)parts(0xff,n),0));
             drawWindows(windowsss);

            while (1==1)
            {
                Mmouse=drawcursor();


                    
                

            }
            Console.ReadKey();
           
        }
    }
}
