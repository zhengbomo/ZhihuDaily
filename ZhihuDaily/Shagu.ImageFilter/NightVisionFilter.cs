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


using System.Collections.Generic;
using Windows.UI;

namespace Shagu.ImageFilter
{
    public class NightVisionFilter : IImageFilter
    {

        private NoiseFilter noisefx = new NoiseFilter();
        private ImageBlender blender = new ImageBlender();
        private VignetteFilter vignetteFx = new VignetteFilter();
        private GradientMapFilter gradientFx = new GradientMapFilter();

        public NightVisionFilter()
        {
            noisefx.Intensity = 0.15f;

            vignetteFx.Size = 1f;

            List<Color> colors = new List<Color>();
            colors.Add(Colors.Black);
            colors.Add(Colors.Green);
            gradientFx.Map = new Gradient(colors);
            gradientFx.BrightnessFactor = 0.2f;
        }

        //@Override
        public Image process(Image imageIn)
        {
            imageIn = noisefx.process(imageIn);
            imageIn = gradientFx.process(imageIn);
            imageIn = vignetteFx.process(imageIn);
            return imageIn;
        }
    }
}
