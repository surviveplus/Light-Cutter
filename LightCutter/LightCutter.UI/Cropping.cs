using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI
{
    public class Cropping : BindableBase
    {

        /// <summary>
        /// Backing field of KeepGrid property.
        /// </summary>
        private bool valueOfKeepGrid;

        /// <summary>
        /// Gets or sets KeepGrid.
        /// </summary>
        public bool KeepGrid
        {
            get
            {
                return this.valueOfKeepGrid;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfKeepGrid, value );
            } // end set
        } // end property

        /// <summary>
        /// Backing field of KeepSqure property.
        /// </summary>
        private bool valueOfKeepSqure;

        /// <summary>
        /// Gets or sets KeepSqure.
        /// </summary>
        public bool KeepSqure
        {
            get
            {
                return this.valueOfKeepSqure;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfKeepSqure, value);
            } // end set
        } // end property

        public void UpdateBounds()
        {
            var point = this.Point;
            var r = new Rect();
            r.X = Math.Min(point.X, this.StartPoint.X);
            r.Y = Math.Min(point.Y, this.StartPoint.Y);
            r.Width = Math.Abs(point.X - this.StartPoint.X) + 1 / toDevice.X;
            r.Height = Math.Abs(point.Y - this.StartPoint.Y) + 1 / toDevice.Y;

            if (this.IsCropping)
            {
                if (this.KeepGrid)
                {
                    point = new Point(
                        point.X + (this.StartPoint.X < point.X ? -1 : 1) * (r.Width % (16 / toDevice.X)),
                        point.Y + (this.StartPoint.Y < point.Y ? -1 : 1) * (r.Height % (16 / toDevice.X)));

                    r.X = Math.Min(point.X, this.StartPoint.X);
                    r.Y = Math.Min(point.Y, this.StartPoint.Y);
                    r.Width = Math.Abs(point.X - this.StartPoint.X) + 1 / toDevice.X;
                    r.Height = Math.Abs(point.Y - this.StartPoint.Y) + 1 / toDevice.Y;

                } // end if(ctrl)

                if (this.KeepSqure)
                {
                    if (r.Width < r.Height)
                    {
                        //r.Height = r.Width;
                        if (r.Y == point.Y)
                        {
                            point.Y = this.StartPoint.Y - r.Width + 1 / toDevice.Y;
                        }
                        else
                        {
                            point.Y = this.StartPoint.Y + r.Width - 1 / toDevice.Y;
                        }
                    }
                    else
                    {
                        //r.Width = r.Height;
                        if (r.X == point.X)
                        {
                            point.X = this.StartPoint.X - r.Height + 1 / toDevice.X;
                        }
                        else
                        {
                            point.X = this.StartPoint.X + r.Height - 1 / toDevice.X;
                        }
                    }

                    r.X = Math.Min(point.X, this.StartPoint.X);
                    r.Y = Math.Min(point.Y, this.StartPoint.Y);
                    r.Width = Math.Abs(point.X - this.StartPoint.X) + 1 / toDevice.X;
                    r.Height = Math.Abs(point.Y - this.StartPoint.Y) + 1 / toDevice.Y;

                } // end if(shift)

                this.Point = point;
            } // end if(isCropping)

            if (this.IsCropping)
            {
                this.PositionLabelContent =
                    "(" + this.BoundsToDevice.Left + "," + this.BoundsToDevice.Top + ") - (" +
                    Math.Max(this.StartPoint.X, point.X) + "," + Math.Max(this.StartPoint.Y, point.Y) + ") : " +
                    this.BoundsToDevice.Width + " x " + this.BoundsToDevice.Height + " Pixels";
            }
            else
            {
                this.PositionLabelContent = "(" + (this.PointToDevice.X) + "," + (this.PointToDevice.Y) + ")";
            }


            this.Bounds = r;
        }


        /// <summary>
        /// Rate to transform to device. 
        /// </summary>
        public Point toDevice;

        /// <summary>
        /// Backing field of Point property.
        /// </summary>
        private Point valueOfPoint;

        /// <summary>
        /// Gets or sets Point.
        /// </summary>
        public Point Point
        {
            get
            {
                return this.valueOfPoint;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfPoint, value);
                this.PointToDevice = new Point(this.Point.X * this.toDevice.X, this.Point.Y * this.toDevice.Y);
                this.MagnifyingCenter = new Point(this.Point.X - 10 / this.toDevice.X, this.Point.Y - 10 / this.toDevice.Y);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of PointToDevice property.
        /// </summary>
        private Point valueOfPointToDevice;

        /// <summary>
        /// Gets or sets PointToDevice.
        /// </summary>
        public Point PointToDevice
        {
            get
            {
                return this.valueOfPointToDevice;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfPointToDevice , value);
            } // end set
        } // end property 


        /// <summary>
        /// Backing field of StartPoint property.
        /// </summary>
        private Point valueOfStartPoint;

        /// <summary>
        /// Gets or sets StartPoint.
        /// </summary>
        public Point StartPoint
        {
            get
            {
                return this.valueOfStartPoint;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfStartPoint, value);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of Bounds property.
        /// </summary>
        private Rect valueOfBounds;

        /// <summary>
        /// Gets or sets Bounds.
        /// </summary>
        public Rect Bounds
        {
            get{
                return this.valueOfBounds;
            } // end get
            set{
                this.SetProperty(ref this.valueOfBounds, value);

                this.BoundsToDevice = new Rect(
                    this.Bounds.Left * this.toDevice.X,
                    this.Bounds.Top * this.toDevice.Y,
                    this.Bounds.Width * this.toDevice.X,
                    this.Bounds.Height * this.toDevice.Y);

            } // end set
        } // end property

        /// <summary>
        /// Backing field of BoundsToDevice property.
        /// </summary>
        private Rect valueOfBoundsToDevice;

        /// <summary>
        /// Gets or sets BoundsToDevice.
        /// </summary>
        public Rect BoundsToDevice
        {
            get
            {
                return this.valueOfBoundsToDevice;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfBoundsToDevice, value);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of IsCropping property.
        /// </summary>
        private bool valueOfIsCropping;

        /// <summary>
        /// Gets or sets isCripping.
        /// </summary>
        public bool IsCropping
        {
            get
            {
                return this.valueOfIsCropping;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfIsCropping, value);

                if (this.IsCropping)
                {
                    this.LineVisibility = Visibility.Collapsed;
                    this.GuideVisibility = Visibility.Visible;
                }
                else
                {
                    this.LineVisibility = Visibility.Visible;
                    this.GuideVisibility = Visibility.Collapsed;
                }

            } // end set
        } // end property


        /// <summary>
        /// Backing field of LineVisibility property.
        /// </summary>
        private Visibility valueOfLineVisibility = Visibility.Visible;

        /// <summary>
        /// Gets or sets LineVisibility.
        /// </summary>
        public Visibility LineVisibility
        {
            get
            {
                return this.valueOfLineVisibility;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfLineVisibility, value);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of GuideVisibility property.
        /// </summary>
        private Visibility valueOfGuideVisibility = Visibility.Collapsed;

        /// <summary>
        /// Gets or sets GuideVisibility.
        /// </summary>
        public Visibility GuideVisibility
        {
            get
            {
                return this.valueOfGuideVisibility;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfGuideVisibility, value);
            } // end set
        } // end property


        /// <summary>
        /// Backing field of MagnifyingCenter property.
        /// </summary>
        private Point valueOfMagnifyingCenter;

        /// <summary>
        /// Gets or sets magnifyingCenter.
        /// </summary>
        public Point MagnifyingCenter
        {
            get
            {
                return this.valueOfMagnifyingCenter;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfMagnifyingCenter, value);

                this.MagnifyingTransform = new Point(
                    -10 * this.toDevice.X * this.MagnifyingCenter.X,
                    -10 * this.toDevice.Y * this.MagnifyingCenter.Y);

                this.MagnifyingClip = new Rect(this.MagnifyingCenter.X, this.MagnifyingCenter.Y, 210 / (10 * this.toDevice.X), 210 / (10 * this.toDevice.Y));

            } // end set
        } // end property

        /// <summary>
        /// Backing field of MagnifyingTransform property.
        /// </summary>
        private Point valueOfMagnifyingTransform;

        /// <summary>
        /// Gets or sets magnifyingTransform.
        /// </summary>
        public Point MagnifyingTransform
        {
            get
            {
                return this.valueOfMagnifyingTransform;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfMagnifyingTransform, value);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of MagnifyingClip property.
        /// </summary>
        private Rect valueOfMagnifyingClip;

        /// <summary>
        /// Gets or sets magnifyingClip.
        /// </summary>
        public Rect MagnifyingClip
        {
            get
            {
                return this.valueOfMagnifyingClip;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfMagnifyingClip, value);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of GuideLocation property.
        /// </summary>
        private Point valueOfGuideLocation;

        /// <summary>
        /// Gets or sets GuideLocation.
        /// </summary>
        public Point GuideLocation
        {
            get
            {
                return this.valueOfGuideLocation;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfGuideLocation , value);
            } // end set
        } // end property

        

        public void UpdateGuilde(double width, double height, double actualWidth, double actualHeight)
        {
            var guideLocation = new Point();

            if (this.BeLeft)
            {
                this.BeLeft = actualWidth + 10 < this.Point.X;
            }
            else
            {
                this.BeLeft = width <actualWidth + this.Point.X + 10;
            }

            if (this.BeLeft)
            {
                guideLocation.X = this.Point.X - 10 - actualWidth;
            }
            else
            {
                guideLocation.X = this.Point.X + 10;
            }

            if (this.beTop)
            {
                this.beTop = actualHeight + 10 < this.Point.Y;
            }
            else
            {
                beTop = height < actualHeight + this.Point.Y + 10;
            }

            if (this.beTop)
            {
                guideLocation.Y = this.Point.Y - 10 - actualHeight;
            }
            else
            {
                guideLocation.Y = this.Point.Y + 10;
            }

            this.GuideLocation = guideLocation;
        }


        //public bool beLeft = false;
        public bool beTop = false;

        /// <summary>
        /// Backing field of BeLeft property.
        /// </summary>
        private bool valueOfBeLeft;

        /// <summary>
        /// Gets or sets beLeft.
        /// </summary>
        public bool BeLeft
        {
            get
            {
                return this.valueOfBeLeft;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfBeLeft , value);

                if (this.BeLeft)
                {
                    this.GuideMessageRightVisibility = Visibility.Collapsed;
                    this.GuideMessageLeftVisibility = Visibility.Visible;
                }
                else
                {
                    this.GuideMessageRightVisibility = Visibility.Visible;
                    this.GuideMessageLeftVisibility = Visibility.Collapsed;
                }
            } // end set
        } // end property

        /// <summary>
        /// Backing field of GuideMessageRightVisibility property.
        /// </summary>
        private Visibility valueOfGuideMessageRightVisibility;

        /// <summary>
        /// Gets or sets GuideMessageRightVisibility.
        /// </summary>
        public Visibility GuideMessageRightVisibility
        {
            get
            {
                return this.valueOfGuideMessageRightVisibility;
            } // end get
            set
            {
                this.SetProperty (ref this.valueOfGuideMessageRightVisibility , value);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of GuideMessageLeftVisibility property.
        /// </summary>
        private Visibility valueOfGuideMessageLeftVisibility;

        /// <summary>
        /// Gets or sets GuideMessageLeftVisibility.
        /// </summary>
        public Visibility GuideMessageLeftVisibility
        {
            get
            {
                return this.valueOfGuideMessageLeftVisibility;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfGuideMessageLeftVisibility , value);
            } // end set
        } // end property

        /// <summary>
        /// Backing field of PositionLabelContent  property.
        /// </summary>
        private String valueOfPositionLabelContent ;

        /// <summary>
        /// Gets or sets PositionLabelContent .
        /// </summary>
        public String PositionLabelContent 
        {
            get
            {
                return this.valueOfPositionLabelContent ;
            } // end get
            set
            {
                this.SetProperty(ref this.valueOfPositionLabelContent ,value );
            } // end set
        } // end property
        
    }
}
