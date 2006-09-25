/*
 * MS	06-05-24	initial version
 * MS	06-09-22	changed using TypeCode instead of if..else
 * MS	06-09-26	improved performance using StringBuilder
 * 
 * 
 */
using System;
using System.Text;
using System.Data;
using System.Globalization;

namespace AjaxPro
{
	/// <summary>
	/// Provides methods to serialize and deserialize a primitive object.
	/// </summary>
	public class PrimitiveConverter : IJavaScriptConverter
	{
		public PrimitiveConverter()
			: base()
		{
			m_serializableTypes = new Type[] {
				typeof(Boolean),
				typeof(Byte), typeof(SByte),
				typeof(Int16), typeof(UInt16), typeof(Int32), typeof(UInt32), typeof(Int64), typeof(UInt64),
				// typeof(Char),		// Char is handeled by StringConverter
				typeof(Double), typeof(Single)
			};

			m_deserializableTypes = m_serializableTypes;
		}

		public override string Serialize(object o)
		{
			StringBuilder sb = new StringBuilder();
			Serialize(o, sb);
			return sb.ToString();
		}

		public override void Serialize(object o, StringBuilder sb)
		{
			if (o is Boolean)
				if ((bool)o == true) sb.Append("true"); else sb.Append("false");
			else if (o is Single)
				sb.Append(((Single)o).ToString(null, CultureInfo.InvariantCulture));
			else if (o is Double)
				sb.Append(((Double)o).ToString(null, CultureInfo.InvariantCulture));
			else
				sb.Append(o.ToString());

			//// Shave off trailing zeros and decimal point, if possible
			//string s = o.ToString().ToLower().Replace(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator, ".");
			//if (s.IndexOf('e') < 0 && s.IndexOf('.') > 0)
			//{
			//    while (s.EndsWith("0"))
			//    {
			//        s.Substring(0, s.Length - 1);
			//    }
			//    if (s.EndsWith("."))
			//    {
			//        s.Substring(0, s.Length - 1);
			//    }
			//}
		}

		public override object Deserialize(IJavaScriptObject o, Type t)
		{
			// TODO: return the default value for this primitive data type
		
			if (!t.IsPrimitive)
				throw new NotSupportedException();

			switch(Type.GetTypeCode(t))
			{
				case TypeCode.Boolean:
					return (bool)((JavaScriptBoolean)o);

				case TypeCode.Byte:
					return Byte.Parse(o.ToString());

				case TypeCode.SByte:
					return SByte.Parse(o.ToString());

				case TypeCode.Int16:
					return Int16.Parse(o.ToString());

				case TypeCode.Int32:
					return Int32.Parse(o.ToString());

				case TypeCode.Int64:
					return Int64.Parse(o.ToString());

				case TypeCode.UInt16:
					return UInt16.Parse(o.ToString());

				case TypeCode.UInt32:
					return UInt32.Parse(o.ToString());

				case TypeCode.UInt64:
					return UInt64.Parse(o.ToString());

				case TypeCode.Single:
					return Single.Parse(o.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));

				case TypeCode.Double:
					return Double.Parse(o.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));

				default:
					throw new NotImplementedException("This primitive data type '" + t.FullName + "' is not implemented.");
			}
		}
	}
}
