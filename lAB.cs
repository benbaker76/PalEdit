using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PalEdit
{
    public class Lab
    {
        private const double Pi2 = 6.283185307179586476925286766559;        //2 * Pi

        public static double GetDeltaE_CIEDE2000(CIELab Color1, CIELab Color2)
        {
            double var1 = (Color1.L + Color2.L) / 2d;   //L_
            double var2 = Math.Sqrt(Math.Pow(Color1.A, 2) + Math.Pow(Color1.B, 2));   //C1
            double var3 = Math.Sqrt(Math.Pow(Color2.A, 2) + Math.Pow(Color2.B, 2));   //C2
            double var4 = (var2 + var3) / 2d;   //C_
            double var5 = (1 - Math.Sqrt((Math.Pow(var4, 7)) / (Math.Pow(var4, 7) + 6103515625))) / 2d;   //G
            double var6 = Color1.A * (1 + var5);   //a1'
            double var7 = Color2.A * (1 + var5);   //a2'
            double var8 = Math.Sqrt(Math.Pow(var6, 2) + Math.Pow(Color1.B, 2));   //C1'
            double var9 = Math.Sqrt(Math.Pow(var7, 2) + Math.Pow(Color2.B, 2));   //C2'
            double var10 = (var8 + var9) / 2d;  //C'_
            double var11 = Math.Atan2(Color1.B, var6);  //h1'
            var11 = (var11 < 0) ? var11 + Pi2 : (var11 >= Pi2) ? var11 - Pi2 : var11;
            double var12 = Math.Atan2(Color2.B, var7);  //h2'
            var12 = (var12 < 0) ? var12 + Pi2 : (var12 >= Pi2) ? var12 - Pi2 : var12;
            double var13 = (Math.Abs(var11 - var12) > Math.PI) ? (var11 + var12 + Pi2) / 2d : (var11 + var12) / 2d;  //H'_
            double var14 = 1 - 0.17 * Math.Cos(var13 - 0.5236) + 0.24 * Math.Cos(2 * var13) + 0.32 * Math.Cos(3 * var13 + 0.10472) - 0.2 * Math.Cos(4 * var13 - 1.0995574);  //T
            double var15 = var12 - var11;  //Delta h'
            var15 = (Math.Abs(var15) > Math.PI && var12 <= var11) ? var15 + Pi2 : (Math.Abs(var15) > Math.PI && var12 > var11) ? var15 - Pi2 : var15;
            double var16 = 2 * Math.Sqrt(var8 * var9) * Math.Sin(var15 / 2d);  //Delta H'
            double var17 = 1 + ((0.015 * Math.Pow(var1 - 50, 2)) / (Math.Sqrt(20 + Math.Pow(var1 - 50, 2))));  //SL
            double var18 = 1 + 0.045 * var10;  //SC
            double var19 = 1 + 0.015 * var10 * var14;  //SH            
            double var20 = 1.0471976 * Math.Exp(-Math.Pow((var13 - 4.799655) / 0.436332313, 2));  //Delta O
            double var21 = 2 * Math.Sqrt(Math.Pow(var10, 7) / (Math.Pow(var10, 7) + 6103515625));  //RC            
            double var22 = -var21 * Math.Sin(2 * var20);  //RT

            return Math.Sqrt(Math.Pow((Color2.L - Color1.L) / var17, 2) + Math.Pow((var9 - var8) / var18, 2) + Math.Pow(var16 / var19, 2) + var22 * ((var9 - var8) / var18) * ((var16) / var19));
        }

        public static double GetDeltaE_CMC(CIELab nLab1, CIELab nLab2)
        {
            double var1 = 1;
            double var2 = Math.Sqrt(Math.Pow(nLab1.A, 2) + Math.Pow(nLab1.B, 2));   //C1
            double var3 = Math.Sqrt(Math.Pow(nLab2.A, 2) + Math.Pow(nLab2.B, 2));   //C2
            double var4 = Math.Pow(nLab1.A - nLab2.A, 2) + Math.Pow(nLab1.B - nLab2.B, 2) - Math.Pow(var2 - var3, 2);
            double var5 = (var4 < 0) ? 0 : Math.Sqrt(var4);   //Delta H
            double var6 = (nLab1.L < 16) ? 0.511 : (nLab1.L * 0.040975) / (1 + nLab1.L * 0.01765);   //SL
            double var7 = ((0.0638 * var2) / (1 + 0.0131 * var2)) + 0.638;   //SC
            double var8 = Math.Atan2(nLab1.B, nLab1.A);   //H1
            var8 = (var8 < 0) ? var8 + Pi2 : (var8 >= Pi2) ? var8 - Pi2 : var8;
            double var9 = (var8 <= 6.0213859193804370403867331512857 && var8 >= 2.8623399732707005061548528603213) ? 0.56 + Math.Abs(0.2 * Math.Cos(var8 + 2.9321531433504736892318004910609)) : 0.36 + Math.Abs(0.4 * Math.Cos(var8 + 0.61086523819801535192329176897101));   //T
            double var10 = Math.Sqrt(Math.Pow(var2, 4) / (Math.Pow(var2, 4) + 1900));  //F
            double var11 = var7 * (var10 * var9 + 1 - var10);  //SH

            return Math.Sqrt(Math.Pow((nLab1.L - nLab2.L) / (var1 * var6), 2) + Math.Pow((var2 - var3) / var7, 2) + Math.Pow(var5 / var11, 2));
        }

        public static CIEXYZ RGBtoXYZ(Color color)
        {
            return RGBtoXYZ(color.R, color.G, color.B);
        }

        public static CIEXYZ RGBtoXYZ(int red, int green, int blue)
        {
            // normalize red, green, blue values
            double rLinear = (double)red / 255.0;
            double gLinear = (double)green / 255.0;
            double bLinear = (double)blue / 255.0;

            // convert to a sRGB form
            double r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (1 + 0.055), 2.2) : (rLinear / 12.92);
            double g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (1 + 0.055), 2.2) : (gLinear / 12.92);
            double b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (1 + 0.055), 2.2) : (bLinear / 12.92);

            // converts
            return new CIEXYZ(
                (r * 0.4124 + g * 0.3576 + b * 0.1805),
                (r * 0.2126 + g * 0.7152 + b * 0.0722),
                (r * 0.0193 + g * 0.1192 + b * 0.9505)
                );
        }

        private static double Fxyz(double t)
        {
            return ((t > 0.008856) ? Math.Pow(t, (1.0 / 3.0)) : (7.787 * t + 16.0 / 116.0));
        }

        public static CIELab XYZtoLab(CIEXYZ xyz)
        {
            return XYZtoLab(xyz.X, xyz.Y, xyz.Z);
        }

        public static CIELab XYZtoLab(double x, double y, double z)
        {
            CIELab lab = CIELab.Empty;

            lab.L = 116.0 * Fxyz(y / CIEXYZ.D65.Y) - 16;
            lab.A = 500.0 * (Fxyz(x / CIEXYZ.D65.X) - Fxyz(y / CIEXYZ.D65.Y));
            lab.B = 200.0 * (Fxyz(y / CIEXYZ.D65.Y) - Fxyz(z / CIEXYZ.D65.Z));

            return lab;
        }

        public static CIELab RGBtoLab(Color color)
        {
            return XYZtoLab(RGBtoXYZ(color));
        }

        public static CIELab RGBtoLab(int red, int green, int blue)
        {
            CIEXYZ xyz = RGBtoXYZ(red, green, blue);

            return XYZtoLab(xyz.X, xyz.Y, xyz.Z);
        }
    }

    /// <summary>
    /// Structure to define CIE XYZ.
    /// </summary>
    public struct CIEXYZ
    {
        /// <summary>
        /// Gets an empty CIEXYZ structure.
        /// </summary>
        public static readonly CIEXYZ Empty = new CIEXYZ();
        /// <summary>
        /// Gets the CIE D65 (white) structure.
        /// </summary>
        public static readonly CIEXYZ D65 = new CIEXYZ(0.9505, 1.0, 1.0890);


        private double x;
        private double y;
        private double z;

        public static bool operator ==(CIEXYZ item1, CIEXYZ item2)
        {
            return (
                item1.X == item2.X
                && item1.Y == item2.Y
                && item1.Z == item2.Z
                );
        }

        public static bool operator !=(CIEXYZ item1, CIEXYZ item2)
        {
            return (
                item1.X != item2.X
                || item1.Y != item2.Y
                || item1.Z != item2.Z
                );
        }

        /// <summary>
        /// Gets or sets X component.
        /// </summary>
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = (value > 0.9505) ? 0.9505 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Y component.
        /// </summary>
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = (value > 1.0) ? 1.0 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Z component.
        /// </summary>
        public double Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = (value > 1.089) ? 1.089 : ((value < 0) ? 0 : value);
            }
        }

        public CIEXYZ(double x, double y, double z)
        {
            this.x = (x > 0.9505) ? 0.9505 : ((x < 0) ? 0 : x);
            this.y = (y > 1.0) ? 1.0 : ((y < 0) ? 0 : y);
            this.z = (z > 1.089) ? 1.089 : ((z < 0) ? 0 : z);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (CIEXYZ)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

    }

    public struct CIELab
    {
        /// <summary>
        /// Gets an empty CIELab structure.
        /// </summary>
        public static readonly CIELab Empty = new CIELab();

        private double l;
        private double a;
        private double b;


        public static bool operator ==(CIELab item1, CIELab item2)
        {
            return (
                item1.L == item2.L
                && item1.A == item2.A
                && item1.B == item2.B
                );
        }

        public static bool operator !=(CIELab item1, CIELab item2)
        {
            return (
                item1.L != item2.L
                || item1.A != item2.A
                || item1.B != item2.B
                );
        }


        /// <summary>
        /// Gets or sets L component.
        /// </summary>
        public double L
        {
            get
            {
                return this.l;
            }
            set
            {
                this.l = value;
            }
        }

        /// <summary>
        /// Gets or sets a component.
        /// </summary>
        public double A
        {
            get
            {
                return this.a;
            }
            set
            {
                this.a = value;
            }
        }

        /// <summary>
        /// Gets or sets a component.
        /// </summary>
        public double B
        {
            get
            {
                return this.b;
            }
            set
            {
                this.b = value;
            }
        }

        public CIELab(double l, double a, double b)
        {
            this.l = l;
            this.a = a;
            this.b = b;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (CIELab)obj);
        }

        public override int GetHashCode()
        {
            return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
        }
    }
}
