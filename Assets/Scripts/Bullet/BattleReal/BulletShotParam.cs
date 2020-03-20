using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 弾を発射する時のパラメータ。
/// </summary>
[Serializable]
public struct BulletShotParam
{
	/// <summary>
	/// 弾を発射させるキャラ。
	/// </summary>
	public BattleRealCharaController BulletOwner;

	/// <summary>
	/// 弾を発射させるキャラの何番目の弾を発射させるか。
	/// </summary>
	public int BulletIndex;

	/// <summary>
	/// 弾を発射させるキャラの何番目のBulletParamを使用するか。
	/// </summary>
	public int BulletParamIndex;

	/// <summary>
	/// BulletParamの何番目のOrbitalParamを使用するか。
	/// </summary>
	public int OrbitalIndex;

	/// <summary>
	/// どの場所から弾が発射するか。
	/// 指定しない場合、発射するキャラの基準座標から発射する。
	/// </summary>
	public Vector3? Position;

	/// <summary>
	/// どの角度から弾が発射するか。
	/// 指定しない場合、発射するキャラと同じ角度で発射する。
	/// </summary>
	public Vector3? Rotation;

	/// <summary>
	/// どのスケールで発射するか。
	/// 指定しない場合、弾のプレハブと同じスケールで発射する。
	/// </summary>
	public Vector3? Scale;

	/// <summary>
	/// 弾の発射パラメータ。
	/// </summary>
	/// <param name="shotOwner">弾を発射させるキャラ</param>
	public BulletShotParam( BattleRealCharaController shotOwner ) : this( shotOwner, 0 )
	{
	}

	/// <summary>
	/// 弾の発射パラメータ。
	/// </summary>
	/// <param name="shotOwner">弾を発射させるキャラ</param>
	/// <param name="bulletIndex">弾を発射させるキャラの何番目の弾を発射させるか</param>
	public BulletShotParam( BattleRealCharaController shotOwner, int bulletIndex ) : this( shotOwner, bulletIndex, 0, -1 )
	{
	}

	/// <summary>
	/// 弾の発射パラメータ。
	/// </summary>
	/// <param name="shotOwner">弾を発射させるキャラ</param>
	/// <param name="bulletIndex">弾を発射させるキャラの何番目の弾を発射させるか</param>
	/// <param name="bulletParamIndex">弾を発射させるキャラの何番目のBulletParamを使用するか</param>
	/// <param name="orbitalIndex">BulletParamの何番目のOrbitalParamを使用するか</param>
	public BulletShotParam( BattleRealCharaController shotOwner, int bulletIndex, int bulletParamIndex, int orbitalIndex ) : this( shotOwner, bulletIndex, bulletParamIndex, orbitalIndex, null, null, null )
	{
	}

	/// <summary>
	/// 弾の発射パラメータ。
	/// </summary>
	/// <param name="shotOwner">弾を発射させるキャラ</param>
	/// <param name="bulletIndex">弾を発射させるキャラの何番目の弾を発射させるか</param>
	/// <param name="bulletParamIndex">弾を発射させるキャラの何番目のBulletParamを使用するか</param>
	/// <param name="orbitalIndex">BulletParamの何番目のOrbitalParamを使用するか</param>
	/// <param name="position">どの場所から弾が発射するか</param>
	/// <param name="rotation">どの角度から弾が発射するか</param>
	/// <param name="scale">どのスケールで発射するか</param>
	public BulletShotParam( BattleRealCharaController shotOwner, int bulletIndex, int bulletParamIndex, int orbitalIndex, Vector3? position, Vector3? rotation, Vector3? scale )
	{
		BulletOwner = shotOwner;
		BulletIndex = bulletIndex;
		BulletParamIndex = bulletParamIndex;
		OrbitalIndex = orbitalIndex;
		Position = position;
		Rotation = rotation;
		Scale = scale;
	}
}