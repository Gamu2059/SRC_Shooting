/// <summary>
/// レイヤー名を定数で管理するクラス
/// </summary>
public static class LayerName
{
	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = 2;
	public const int Water = 4;
	public const int UI = 5;
	public const int PostProcessing = 8;
	public const int IgnoreLight = 9;
	public const int BattleMainBackEnd = 10;
	public const int BattleMainFrontEnd = 11;
	public const int DefaultMask = 1;
	public const int TransparentFXMask = 2;
	public const int IgnoreRaycastMask = 4;
	public const int WaterMask = 16;
	public const int UIMask = 32;
	public const int PostProcessingMask = 256;
	public const int IgnoreLightMask = 512;
	public const int BattleMainBackEndMask = 1024;
	public const int BattleMainFrontEndMask = 2048;
}
