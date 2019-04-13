using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾やキャラの当たり判定を管理する。
/// </summary>
public class CollisionManager : SingletonMonoBehavior<CollisionManager>
{
	public override void OnUpdate()
	{
		CheckCollision();
	}

	/// <summary>
	/// 衝突判定を行う。
	/// </summary>
	public void CheckCollision()
	{
        OnCheckCollisionBullet();
        OnCheckCollisionChara();
        OnCheckCollisionItem();

        CheckCollisionBulletToBullet();
        CheckCollisionBulletToChara();
        CheckCollisionEnemyToPlayer();
        CheckCollisionPlayerToItem();
	}

    /// <summary>
    /// 弾から弾への衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToBullet()
    {
        var bullets = BulletManager.Instance.GetUpdateBullets();

        foreach (var bullet in bullets)
        {
            if (!bullet.CanHitBullet())
            {
                continue;
            }

            foreach(var targetBullet in bullets)
            {
                if (bullet == targetBullet || bullet.GetTroop() == targetBullet.GetTroop())
                {
                    continue;
                }

                Collision.CheckCollide(bullet, targetBullet, (attackData, targetData)=> {

                });
            }
        }
    }

    /// <summary>
    /// 弾からキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToChara()
    {
        var bullets = BulletManager.Instance.GetUpdateBullets();
        var player = PlayerCharaManager.Instance.GetCurrentController();
        var enemies = EnemyCharaManager.Instance.GetUpdateEnemies();

        foreach (var bullet in bullets)
        {
            if (bullet.GetTroop() == E_CHARA_TROOP.ENEMY)
            {
                OnCharaCollide(bullet, player);
            } else
            {
                foreach(var enemy in enemies)
                {
                    OnCharaCollide(bullet, enemy);
                }
            }
        }
    }

    /// <summary>
    /// 敵キャラからプレイヤーキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionEnemyToPlayer()
    {
        var player = PlayerCharaManager.Instance.GetCurrentController();
        var enemies = EnemyCharaManager.Instance.GetUpdateEnemies();

        foreach (var enemy in enemies)
        {
            OnCheckCollisionEnemyToPlayer(enemy, player);
        }
    }
    
    /// <summary>
    /// プレイヤーキャラからアイテムへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionPlayerToItem()
    {
        var player = PlayerCharaManager.Instance.GetCurrentController();
        var items = ItemManager.Instance.GetUpdateItems();

        foreach (var item in items)
        {
            OnCheckCollisionPlayerToItem(player, item);
        }
    }

	///// <summary>
	///// 弾から弾への詳細な衝突判定を行う。
	///// </summary>
	//private void OnCheckCollisionBulletToBullet( BulletController attack, BulletController target )
	//{
 //       var attackDatas = attack.GetColliderData();
	//	var targetDatas = target.GetColliderData();

	//	foreach( var attackData in attackDatas )
	//	{
	//		foreach( var targetData in targetDatas )
	//		{
	//			bool isHit = Collision.IsCollide( attackData, targetData );

	//			if( isHit )
	//			{
	//				//target.OnSuffer( attack, targetData );
	//				//attack.OnHitBullet( target );
	//			}
	//		}
	//	}
	//}

	///// <summary>
	///// 弾からキャラへの詳細な衝突判定を行う。
	///// </summary>
	//private void OnCheckCollisionBulletToChara( BulletController attack, CharaController target)
	//{
 //       var attackDatas = attack.GetColliderData();
	//	var targetDatas = target.GetColliderData();

	//	foreach( var baseData in attackDatas )
	//	{
	//		foreach( var targetData in targetDatas )
	//		{
	//			bool isHit = Collision.IsCollide( baseData, targetData );

	//			if( isHit )
	//			{
	//				//target.OnSuffer( attack, targetData );
	//				//attack.OnHitCharacter( target );
	//			}
	//		}
	//	}
	//}

 //   /// <summary>
 //   /// 敵キャラからプレイヤーキャラへの詳細な衝突判定を行う。
 //   /// </summary>
 //   private void OnCheckCollisionEnemyToPlayer(CharaController attack, CharaController target)
 //   {
 //       var attackDatas = attack.GetColliderData();
 //       var targetDatas = target.GetColliderData();

 //       foreach(var baseData in attackDatas)
 //       {
 //           foreach(var targetData in targetDatas)
 //           {
 //               bool isHit = Collision.IsCollide(baseData, targetData);

 //               if (isHit)
 //               {
 //                   target.OnSufferChara(attack, baseData, targetData);
 //                   attack.OnHitChara(target, baseData, targetData);
 //               }
 //           }
 //       }
 //   }

 //   private void OnCheckCollisionPlayerToItem(CharaController attack, ItemController target)
 //   {
 //       var charaDatas = attack.GetColliderData();
 //       var itemDatas = target.GetColliderData();

        
 //   }
}
