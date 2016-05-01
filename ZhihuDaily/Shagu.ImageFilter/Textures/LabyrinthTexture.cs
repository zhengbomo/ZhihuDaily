// AForge Image Processing Library
//
// Copyright ?Andrew Kirillov, 2005-2007
// andrew.kirillov@gmail.com
//

using System;

namespace Shagu.ImageFilter.Textures
{
    /// <summary>
	/// Labirinth texture
	/// </summary>
    /// 
    /// <remarks></remarks>
    /// 
	public class LabyrinthTexture : ITextureGenerator
	{
        // Perlin noise function used for texture generation
        private PerlinNoise noise = new PerlinNoise( 1.0 / 16, 1.0, 0.65, 1 );

        // randmom number generator
        private Random rand = new Random( (int) DateTime.Now.Ticks );
        private int r;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabyrinthTexture"/> class
        /// </summary>
        /// 
		public LabyrinthTexture( )
		{
			Reset( );
		}

        /// <summary>
        /// Generate texture
        /// </summary>
        /// 
        /// <param name="width">Texture's width</param>
        /// <param name="height">Texture's height</param>
        /// 
        /// <returns>Two dimensional array of intensities</returns>
        /// 
        /// <remarks>Generates new texture with specified dimension.</remarks>
        /// 
        public float[,] Generate( int width, int height )
		{
			float[,] texture = new float[height, width];

			for ( int y = 0; y < height; y++ )
			{
				for ( int x = 0; x < width; x++ )
				{
					texture[y, x] =
                        System.Math.Min(1.0f,
							(float) System.Math.Abs( noise.Function2D( x + r, y + r ) )
						);

				}
			}
			return texture;
		}

        /// <summary>
        /// Reset generator
        /// </summary>
        /// 
        /// <remarks>Regenerates internal random numbers.</remarks>
        /// 
        public void Reset( )
		{
			r = rand.Next( 5000 );
		}
	}
}
