using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// 文字列をパラメータに変換するクラス。
/// </summary>
public class StringParamTranslator
{
	public static StringParamSet TranslateString( string paramData )
	{
		var paramSet = new StringParamSet();

		if( paramData == null )
		{
			return null;
		}

		string[] paramDataArray = paramData.Split( ';' );

		foreach( var data in paramDataArray )
		{
			string[] pArray = data.Trim().Split( ':' );

			if( pArray.Length != 3 )
			{
				continue;
			}

			string type = pArray[0].Trim();
			string name = pArray[1].Trim();
			string value = pArray[2].Trim();

			switch( type )
			{
				case "I":
					AddIntParam( paramSet, name, value );
					break;

				case "F":
					AddFloatParam( paramSet, name, value );
					break;

				case "D":
					AddDoubleParam( paramSet, name, value );
					break;

				case "B":
					AddBoolParam( paramSet, name, value );
					break;

				case "V2":
					AddVector2Param( paramSet, name, value );
					break;

				case "V3":
					AddVector3Param( paramSet, name, value );
					break;
			}
		}

		return paramSet;
	}

	private static void AddIntParam( StringParamSet set, string name, string value )
	{
		int intR;

		if( int.TryParse( value, out intR ) )
		{
			if( set.IntParam == null )
			{
				set.IntParam = new Dictionary<string, int>();
			}

			set.IntParam.Add( name, intR );
		}
	}

	private static void AddFloatParam( StringParamSet set, string name, string value )
	{
		float floatR;

		if( float.TryParse( value, out floatR ) )
		{
			if( set.FloatParam == null )
			{
				set.FloatParam = new Dictionary<string, float>();
			}

			set.FloatParam.Add( name, floatR );
		}
	}

	private static void AddDoubleParam( StringParamSet set, string name, string value )
	{
		double doubleR;

		if( double.TryParse( value, out doubleR ) )
		{
			if( set.DoubleParam == null )
			{
				set.DoubleParam = new Dictionary<string, double>();
			}

			set.DoubleParam.Add( name, doubleR );
		}
	}

	private static void AddBoolParam( StringParamSet set, string name, string value )
	{
		bool boolR;

		if( bool.TryParse( value, out boolR ) )
		{
			if( set.BoolParam == null )
			{
				set.BoolParam = new Dictionary<string, bool>();
			}

			set.BoolParam.Add( name, boolR );
		}
	}

	private static void AddVector2Param( StringParamSet set, string name, string value )
	{
		// ()内の文字列を抽出
		Match match = Regex.Match( value, "\\((.+)\\)" );
		string inValue = match.Result( "$1" );

		if( inValue == null )
		{
			return;
		}

		string[] pArray = inValue.Split( ',' );
		float x;
		float y;

		float.TryParse( pArray[0].Trim(), out x );
		float.TryParse( pArray[1].Trim(), out y );

		if( set.V2Param == null )
		{
			set.V2Param = new Dictionary<string, Vector2>();
		}

		set.V2Param.Add( name, new Vector2( x, y ) );
	}

	private static void AddVector3Param( StringParamSet set, string name, string value )
	{
		// ()内の文字列を抽出
		Match match = Regex.Match( value, "\\((.+)\\)" );
		string inValue = match.Result( "$1" );

		if( inValue == null )
		{
			return;
		}

		string[] pArray = inValue.Split( ',' );
		float x;
		float y;
		float z;

		float.TryParse( pArray[0].Trim(), out x );
		float.TryParse( pArray[1].Trim(), out y );
		float.TryParse( pArray[2].Trim(), out z );

		if( set.V3Param == null )
		{
			set.V3Param = new Dictionary<string, Vector3>();
		}

		set.V3Param.Add( name, new Vector3( x, y, z ) );
	}
}
