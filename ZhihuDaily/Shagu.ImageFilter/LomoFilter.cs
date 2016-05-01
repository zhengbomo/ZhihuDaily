/* 
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

namespace Shagu.ImageFilter
{
    public class LomoFilter : IImageFilter
    {
        private BrightContrastFilter contrastFx = new BrightContrastFilter();
        private GradientMapFilter gradientMapFx  = new GradientMapFilter();
        private ImageBlender blender = new ImageBlender();
        private VignetteFilter vignetteFx = new VignetteFilter();
        private NoiseFilter noiseFx = new NoiseFilter();

        public LomoFilter()
        {
            contrastFx.BrightnessFactor = 0.05f;
            contrastFx.ContrastFactor = 0.5f;
         
            blender.Mixture = 0.5f;
            blender.Mode = BlendMode.Multiply;
        
            vignetteFx.Size = 0.6f;

            noiseFx.Intensity = 0.02f;
        }

        public Image process(Image imageIn)
        {
            Image tempImg = contrastFx.process(imageIn);
            tempImg = this.noiseFx.process(tempImg);
            imageIn = this.gradientMapFx.process(tempImg);
            imageIn = this.blender.Blend(imageIn, tempImg);
            imageIn = this.vignetteFx.process(imageIn);
            return tempImg;
        }
    }
}
