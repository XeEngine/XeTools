using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Services
{
    /// <summary>Insufficient space left in packing area to contain a given object</summary>
    /// <remarks>
    ///   An exception being sent to you from deep space. Erm, no, wait, it's an exception
    ///   that occurs when a packing algorithm runs out of space and is unable to fit
    ///   the object you tried to pack into the remaining packing area.
    /// </remarks>
    [Serializable]
    internal class OutOfSpaceException : Exception
    {
        /// <summary>Initializes the exception with an error message</summary>
        /// <param name="message">Error message describing the cause of the exception</param>
        public OutOfSpaceException(string message)
            : base(message)
        {
        }
    }

    /// <summary>Base class for rectangle packing algorithms</summary>
    /// <remarks>
    ///   <para>
    ///     By uniting all rectangle packers under this common base class, you can
    ///     easily switch between different algorithms to find the most efficient or
    ///     performant one for a given job.
    ///   </para>
    ///   <para>
    ///     An almost exhaustive list of packing algorithms can be found here:
    ///     http://www.csc.liv.ac.uk/~epa/surveyhtml.html
    ///   </para>
    /// </remarks>
    internal abstract class RectanglePacker
    {
        /// <summary>Maximum width the packing area is allowed to have</summary>
        protected int PackingAreaWidth { get; private set; }

        /// <summary>Maximum height the packing area is allowed to have</summary>
        protected int PackingAreaHeight { get; private set; }

        /// <summary>Initializes a new rectangle packer</summary>
        /// <param name="packingAreaWidth">Width of the packing area</param>
        /// <param name="packingAreaHeight">Height of the packing area</param>
        protected RectanglePacker(int packingAreaWidth, int packingAreaHeight)
        {
            PackingAreaWidth = packingAreaWidth;
            PackingAreaHeight = packingAreaHeight;
        }

        /// <summary>Allocates space for a rectangle in the packing area</summary>
        /// <param name="rectangleWidth">Width of the rectangle to allocate</param>
        /// <param name="rectangleHeight">Height of the rectangle to allocate</param>
        /// <returns>The location at which the rectangle has been placed</returns>
        public virtual Pointi Pack(int rectangleWidth, int rectangleHeight)
        {

            if (!TryPack(rectangleWidth, rectangleHeight, out Pointi point))
                throw new OutOfSpaceException("Rectangle does not fit in packing area");

            return point;
        }

        /// <summary>Tries to allocate space for a rectangle in the packing area</summary>
        /// <param name="rectangleWidth">Width of the rectangle to allocate</param>
        /// <param name="rectangleHeight">Height of the rectangle to allocate</param>
        /// <param name="placement">Output parameter receiving the rectangle's placement</param>
        /// <returns>True if space for the rectangle could be allocated</returns>
        public abstract bool TryPack(int rectangleWidth, int rectangleHeight, out Pointi placement);
    }
}
