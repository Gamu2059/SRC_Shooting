using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLaser : Bullet
{

	[SerializeField]
	private Vector3 m_OffsetPos;

	public override void ChangeOrbital( BulletOrbitalParam orbitalParam )
	{
		base.ChangeOrbital( orbitalParam );

		OptionValueParam[] valueParams = orbitalParam.OptionValueParams;

		foreach( var value in valueParams )
		{
			if( value.Key == "Offset X" )
			{
				m_OffsetPos.x = value.Value;
			}

			if( value.Key == "Offset Z" )
			{
				m_OffsetPos.z = value.Value;
			}
		}
	}

	protected override void OnUpdateBulletOrbital()
	{
		base.OnUpdateBulletOrbital();

		transform.position = m_Owner.transform.position;
		transform.Translate( m_OffsetPos, Space.Self );
	}

}
