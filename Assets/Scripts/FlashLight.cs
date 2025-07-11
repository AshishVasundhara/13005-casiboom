using DG.Tweening;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
	public Transform mTransform;

	private void Start()
	{
		InvokeRepeating("LaunchProjectile", 4f, 15f);
	}

	private void LaunchProjectile()
	{
		Cells.instance.SetSpriteMask(isActive: true);
		mTransform.DOMove(new Vector3(4.42f, -3.66f, -1.1f), 4f).OnComplete(delegate
		{
			mTransform.position = new Vector3(-4.72f, 5.48f, -1.1f);
			Cells.instance.SetSpriteMask(isActive: false);
		});
	}
}
