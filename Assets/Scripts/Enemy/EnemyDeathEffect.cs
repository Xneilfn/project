using System.Collections;
using UnityEngine;

/// <summary>
/// Эффект частиц при смерти врага — создаётся полностью кодом.
/// Вызывается из EnemyStats.Kill().
/// </summary>
public class EnemyDeathEffect : MonoBehaviour
{
    public static void Spawn(Vector3 position, Color color)
    {
        GameObject go = new GameObject("DeathEffect");
        go.transform.position = position;
        go.AddComponent<EnemyDeathEffect>().Init(color);
    }

    void Init(Color color)
    {
        ParticleSystem ps = gameObject.AddComponent<ParticleSystem>();

        // Останавливаем чтобы настроить
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration          = 0.5f;
        main.loop              = false;
        main.startLifetime     = new ParticleSystem.MinMaxCurve(0.3f, 0.7f);
        main.startSpeed        = new ParticleSystem.MinMaxCurve(2f, 5f);
        main.startSize         = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);
        main.startColor        = new ParticleSystem.MinMaxGradient(color, Color.yellow);
        main.gravityModifier   = 0.5f;
        main.simulationSpace   = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, 15)  // 15 частиц сразу
        });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius    = 0.1f;

        // Рендерер
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.sortingLayerName = "Default";
        renderer.sortingOrder     = 10;

        ps.Play();
        Destroy(gameObject, 1.5f);
    }
}
