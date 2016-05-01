﻿/* 
 * HaoRan ImageFilter Classes v0.1
 * Copyright (C) 2012 Zhenjun Dai
 *
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by the
 * Free Software Foundation; either version 2.1 of the License, or (at your
 * option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License
 * for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library; if not, write to the Free Software Foundation.
 */

using System;

namespace Shagu.ImageFilter
{
    public class LightFilter : RadialDistortionFilter
    {

        /**
         * 光照亮度
         */
        public float Light = 150.0f;

        //@Override
        public override Image process(Image imageIn)
        {
            int width = imageIn.getWidth();
            int halfw = width / 2;
            int height = imageIn.getHeight();
            int halfh = height / 2;
            int R = System.Math.Min(halfw, halfh);
            //中心点，发亮此值会让强光中心发生偏移
            Point point = new Point(halfw, halfh);
            int r = 0, g = 0, b = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float length = (float)System.Math.Sqrt(System.Math.Pow((x - point.X), 2) + System.Math.Pow((y - point.Y), 2));
                    r = imageIn.getRComponent(x, y);
                    g = imageIn.getGComponent(x, y);
                    b = imageIn.getBComponent(x, y);
                    //位于光晕中
                    if (length < R)
                    {
                        float pixel = Light * (1.0f - length / R);
                        r = r + (int)pixel;
                        r = System.Math.Max(0, System.Math.Min(r, 255));
                        g = g + (int)pixel;
                        g = System.Math.Max(0, System.Math.Min(g, 255));
                        b = b + (int)pixel;
                        b = System.Math.Max(0, System.Math.Min(b, 255));
                    }
                    imageIn.setPixelColor(x, y, r, g, b);
                }
            }
            return imageIn;
        }
    }

}