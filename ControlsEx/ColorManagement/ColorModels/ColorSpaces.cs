using System;
using System.Drawing;
using System.ComponentModel;

namespace ControlsEx.ColorManagement.ColorModels
{
	/// <summary>
	/// CIE XYZ color space
	/// </summary>
	[Serializable, TypeConverter(typeof(XYZTypeConverter))]
	public struct XYZ
	{
		public static readonly XYZ Empty=new XYZ();
		public static readonly XYZ White=new XYZ(95.047,100.000,108.883);
		#region variables
		private double m_x, m_y, m_z;
		#endregion
		#region ctor
		public XYZ(double x, double y, double z)
		{
			m_x=ClipValue(x,0.0,95.047);
			m_y=ClipValue(y,0.0,100.000);
			m_z=ClipValue(z,0.0,108.883);
		}
		public static XYZ FromRGB(RGB value)
		{
			double
				r=GammaCorrection(value.R),
				g=GammaCorrection(value.G),
				b=GammaCorrection(value.B);
			//Observer. = 2°, Illuminant = D65
			return new XYZ(
				r*41.24+g*35.76+b*18.05,//multiplicated by 100
				r*21.26+g*71.52+b*7.22,
				r*1.93+g*11.92+b*95.05);
		}
		#endregion
		#region static functions
		public static double ClipValue(double value, double min, double max)
		{
			if(double.IsNaN(value) ||
				double.IsNegativeInfinity(value) ||
				value<min)
				return min;
			else if(double.IsPositiveInfinity(value) ||
				value>max)
				return max;
			else return value;
		}
		private static double GammaCorrection(double value)
		{
			if (value>0.04045)
				return Math.Pow((value+0.055)/1.055,2.4);
			else
				return value/12.92;
		}
		private static double InvertGammaCorrection(double value)
		{
			if (value>0.0031308)
				return 1.055*Math.Pow(value,1.0/2.4)-0.055;
			else
				return 12.92*value;
		}
		#endregion
		#region operators
		public static bool operator ==(XYZ a, XYZ b)
		{
			return
				a.m_x==b.m_x &&
				a.m_y==b.m_y &&
				a.m_z==b.m_z;
		}
		public static bool operator !=(XYZ a, XYZ b)
		{
			return !(a==b);
		}
		public override bool Equals(object obj)
		{
			if(obj is XYZ)
			{
				return ((XYZ)obj)==this;
			}
			return false;
		}
		public override int GetHashCode()
		{
			string representation=
				m_x.ToString()+":"+
				m_y.ToString()+":"+
				m_z.ToString();
			return representation.GetHashCode ();
		}
		#endregion
		#region conversion
		public RGB ToRGB()
		{
			//Observer. = 2°, Illuminant = D65
			double
				r=InvertGammaCorrection(m_x*+0.032406+m_y*-0.015372+m_z*-0.004986),
				g=InvertGammaCorrection(m_x*-0.009689+m_y*+0.018758+m_z*+0.000415),
				b=InvertGammaCorrection(m_x*+0.000557+m_y*-0.002040+m_z*+0.010570);
			return new RGB(r,g,b);
		}
		public override string ToString()
		{
			return String.Format("CIE-XYZ[\nX={0};\tY={1};\tZ={2}\n]",
				m_x,m_y,m_z);
		}
		#endregion
		#region properties
		public double X
		{
			get{return m_x;}
			set{m_x=ClipValue(value,0.0,95.047);}
		}
		public double Y
		{
			get{return m_y;}
			set{m_y=ClipValue(value,0.0,100.000);}
		}
		public double Z
		{
			get{return m_z;}
			set{m_z=ClipValue(value,0.0,108.883);}
		}
		#endregion
	}
	[Serializable]
	public struct RGB
	{
		#region variables
		private double m_r, m_g, m_b;
		#endregion
		#region ctor
		public RGB(double r, double g, double b)
		{
			m_r=XYZ.ClipValue(r,0.0,1.0);
			m_g=XYZ.ClipValue(g,0.0,1.0);
			m_b=XYZ.ClipValue(b,0.0,1.0);
		}
		public RGB(Color value):
			this(
			(double)(value.R)/255.0,
			(double)(value.G)/255.0,
			(double)(value.B)/255.0){}
		#endregion
		#region operators
		public static bool operator ==(RGB a, RGB b)
		{
			return
				a.m_r==b.m_r &&
				a.m_g==b.m_g &&
				a.m_b==b.m_b;
		}
		public static bool operator !=(RGB a, RGB b)
		{
			return !(a==b);
		}
		public override bool Equals(object obj)
		{
			if(obj is RGB)
			{
				return ((RGB)obj)==this;
			}
			return false;
		}
		public override int GetHashCode()
		{
			string representation=
				m_r.ToString()+":"+
				m_g.ToString()+":"+
				m_b.ToString();
			return representation.GetHashCode ();
		}
		#endregion
		#region conversion
		public static implicit operator Color(RGB value)
		{
			return value.ToArgb();
		}
		public static implicit operator RGB(Color value)
		{
			return new RGB(value);
		}
		public Color ToArgb()
		{
			return Color.FromArgb(
				(int)Math.Round(255.0*m_r),
				(int)Math.Round(255.0*m_g),
				(int)Math.Round(255.0*m_b));
		}
		#endregion
		#region properties
		public double R
		{
			get{return m_r;}
			set{m_r=XYZ.ClipValue(value,0.0,1.0);}
		}
		public double G
		{
			get{return m_g;}
			set{m_g=XYZ.ClipValue(value,0.0,1.0);}
		}
		public double B
		{
			get{return m_b;}
			set{m_b=XYZ.ClipValue(value,0.0,1.0);}
		}
		#endregion
	}
	[Serializable]
	public struct LAB
	{
		#region variables
		private double m_l, m_a, m_b;
		#endregion
		#region ctor
		public LAB(double l, double a, double b)
		{
			m_l=XYZ.ClipValue(l,0.0,100.0);
			m_a=XYZ.ClipValue(a,-128.0,128.0);
			m_b=XYZ.ClipValue(b,-128.0,128.0);
		}
		public static LAB FromXYZ(XYZ value)
		{
			//normalize values
			double x=DriveCurve(value.X/XYZ.White.X),
				y=DriveCurve(value.Y/XYZ.White.Y),
				z=DriveCurve(value.Z/XYZ.White.Z);
			//return value
			return new LAB(
				(116.0*y)-16.0,
				500.0*(x-y),
				200.0*(y-z));
		}
		#endregion
		#region static functions
		private static double DriveCurve(double value)
		{
			if(value>0.008856)return Math.Pow(value,1.0/3.0);
			else return (7.787*value)+(16.0/116.0);
		}
		private static double DriveInverseCurve(double value)
		{
			double cubic=value*value*value;
			if(cubic>0.008856) return cubic;
			else return (value-16.0/116.0)/7.787;
		}
		#endregion
		#region operators
		public static bool operator ==(LAB a, LAB b)
		{
			return 
				a.m_l==b.m_l &&
				a.m_a==b.m_a &&
				a.m_b==b.m_b;
		}
		public static bool operator !=(LAB a,LAB b)
		{
			return !(a==b);
		}
		public override bool Equals(object obj)
		{
			if(obj is LAB)
			{
				return ((LAB)obj)==this;
			}
			return false;
		}
		public override int GetHashCode()
		{
			string representation=
				m_l.ToString()+":"+
				m_a.ToString()+":"+
				m_b.ToString();
			return representation.GetHashCode ();
		}
		#endregion
		#region conversion
		public XYZ ToXYZ()
		{
			double y=(m_l+16.0)/116.0,
				x=m_a/500.0+y,
				z=y-m_b/200.0;
			return new XYZ(
				DriveInverseCurve(x)*XYZ.White.X,
				DriveInverseCurve(y)*XYZ.White.Y,
				DriveInverseCurve(z)*XYZ.White.Z);
		}
		public override string ToString()
		{
			return String.Format("CIE-Lab[\nL={0};\ta={1};\tb={2}\n]",
				m_l,m_a,m_b);
		}
		#endregion
		#region properties
		public double L
		{
			get{return m_l;}
			set{m_l=XYZ.ClipValue(value,0.0,100.0);}
		}
		public double a
		{
			get{return m_a;}
			set{m_a=XYZ.ClipValue(value,-128.0,127.0);}
		}
		public double b
		{
			get{return m_b;}
			set{m_b=XYZ.ClipValue(value,-128.0,127.0);}
		}
		#endregion
	}
	[Serializable]
	public struct HSV
	{
		#region variables
		private double m_h, m_s, m_v;
		#endregion
		#region ctor
		public HSV(double h, double s, double v)
		{
			m_h=XYZ.ClipValue(h,0.0,1.0);
			m_s=XYZ.ClipValue(s,0.0,1.0);
			m_v=XYZ.ClipValue(v,0.0,1.0);
		}
		public static HSV FromRGB(RGB col)
		{
			double
				min = Math.Min( Math.Min(col.R,col.G),col.B),
				max = Math.Max( Math.Max(col.R,col.G),col.B),
				deltam_max = max-min;

			HSV ret=new HSV(0,0,0);
			ret.m_v=max;

			if (deltam_max==0.0)
			{
				ret.m_h=0.0;
				ret.m_s=0.0;
			}
			else
			{
				ret.m_s=deltam_max/max;

				double del_R=(((max-col.R)/6.0)+(deltam_max/2.0))/deltam_max;
				double del_G=(((max-col.G)/6.0)+(deltam_max/2.0))/deltam_max;
				double del_B=(((max-col.B)/6.0)+(deltam_max/2.0))/deltam_max;

				if      (col.R==max) ret.m_h = del_B-del_G;
				else if (col.G==max) ret.m_h = (1.0/3.0)+del_R-del_B;
				else if (col.B==max) ret.m_h = (2.0/3.0)+del_G-del_R;

				if (ret.m_h<0.0)ret.m_h+=1.0;
				if (ret.m_h>1.0)ret.m_h-=1.0;
			}
			return ret;
		}
		#endregion
		#region operators
		public static bool operator==(HSV a, HSV b)
		{
			return
				a.m_h==b.m_h &&
				a.m_s==b.m_s &&
				a.m_v==b.m_v;
		}
		public static bool operator!=(HSV a, HSV b)
		{
			return !(a==b);
		}
		public override bool Equals(object obj)
		{
			if(obj is HSV)
			{
				return ((HSV)obj)==this;
			}
			return false;
		}
		public override int GetHashCode()
		{
			string representation=
				m_h.ToString() + ":" +
				m_s.ToString() + ":" +
				m_v.ToString();
			return representation.GetHashCode();
		}
		#endregion
		#region conversion
		public RGB ToRGB()
		{
			if(m_s==0.0)
			{
				return new RGB(m_v,m_v,m_v);
			}
			else
			{
				double h=m_h*6.0;
				if(h==6.0) h=0.0;
				int h_i=(int)Math.Floor(h);
				double
					var_1=m_v*(1.0-m_s),
					var_2=m_v*(1.0-m_s*(h-h_i)),
					var_3=m_v*(1.0-m_s*(1.0-(h-h_i)));

				double r,g,b;
				switch(h_i)
				{
					case 0:	r=m_v;		g=var_3;	b=var_1;break;
					case 1:	r=var_2;	g=m_v;		b=var_1;break;
					case 2:	r=var_1;	g=m_v;		b=var_3;break;
					case 3:	r=var_1;	g=var_2;	b=m_v;	break;
					case 4:	r=var_3;	g=var_1;	b=m_v;	break;
					default:	r=m_v;		g=var_1;	b=var_2;break;
				}
				return new RGB(r,g,b);
			}
		}
		public override string ToString()
		{
			return String.Format("HSV[\nH={0};\tS={1};\tV={2}\n]",
				m_h,m_s,m_v);
		}
		#endregion
		#region properties
		public double H
		{
			get{return m_h;}
			set{m_h=XYZ.ClipValue(value,0.0,1.0);}
		}
		public double S
		{
			get{return m_s;}
			set{m_s=XYZ.ClipValue(value,0.0,1.0);}
		}
		public double V
		{
			get{return m_v;}
			set{m_v=XYZ.ClipValue(value,0.0,1.0);}
		}
		#endregion
	}
	[Serializable]
	public struct CMYK
	{
		#region variables
		public double m_c, m_m, m_y, m_k;
		#endregion
		#region ctor
		public CMYK(double c, double m, double y, double k)
		{
			m_c=XYZ.ClipValue(c,0.0,1.0);
			m_m=XYZ.ClipValue(m,0.0,1.0);
			m_y=XYZ.ClipValue(y,0.0,1.0);
			m_k=XYZ.ClipValue(k,0.0,1.0);
		}
		public static CMYK FromRGB(RGB value)
		{
			double
				c=1.0-value.R,
				m=1.0-value.G,
				y=1.0-value.B,
				k=1.0;
			if(c<k) k=c;
			if(m<k) k=m;
			if(y<k) k=y;
			if(k==1.0)//black
			{
				c=m=y=0.0;
			}
			else
			{
				c=(c-k)/(1.0-k);
				m=(m-k)/(1.0-k);
				y=(y-k)/(1.0-k);
			}
			return new CMYK(c,m,y,k);
		}
		#endregion
		#region operators
		public static bool operator ==(CMYK a, CMYK b)
		{
			return 
				a.m_c==b.m_c &&
				a.m_k==b.m_k &&
				a.m_m==b.m_m &&
				a.m_y==b.m_y;
		}
		public static bool operator !=(CMYK a, CMYK b)
		{
			return !(a==b);
		}
		public override bool Equals(object obj)
		{
			if(obj is CMYK)
			{
				return ((CMYK)obj)==this;
			}
			return false;
		}
		public override int GetHashCode()
		{
			string representation=
				m_c.ToString() + ":" +
				m_m.ToString() + ":" +
				m_y.ToString() + ":" +
				m_k.ToString();
			return representation.GetHashCode ();
		}
		#endregion
		#region conversion
		public RGB ToRGB()
		{
			double
				c=m_c*(1.0-m_k)+m_k,
				m=m_m*(1.0-m_k)+m_k,
				y=m_y*(1.0-m_k)+m_k;

			return new RGB(1.0-c,1.0-m,1.0-y);
		}
		public override string ToString()
		{
			return String.Format("CMYK[\nC={0};\tM={1};\tY={2};\tK={3}\n]",
				m_c,m_m,m_y,m_k);
		}
		#endregion
		#region properties
		public double C
		{
			get{return m_c;}
			set{m_c=XYZ.ClipValue(value,0.0,1.0);}
		}
		public double M
		{
			get{return m_m;}
			set{m_m=XYZ.ClipValue(value,0.0,1.0);}
		}
		public double Y
		{
			get{return m_y;}
			set{m_y=XYZ.ClipValue(value,0.0,1.0);}
		}
		public double K
		{
			get{return m_k;}
			set{m_k=XYZ.ClipValue(value,0.0,1.0);}
		}
		#endregion
	}
}
