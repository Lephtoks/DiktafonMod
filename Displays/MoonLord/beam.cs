using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace DiktafonMod.Displays.MoonLord;

public class MoonLordBeam : ModDisplay
{
    private static readonly int Color1 = Shader.PropertyToID("_Color1");
    private static readonly int Color1Power = Shader.PropertyToID("_Color1Power");
    private static readonly int Color2 = Shader.PropertyToID("_Color2");
    private static readonly int BeamPower = Shader.PropertyToID("_BeamPower");

    public override string BaseDisplay => "b740dac7b6bc850438df2a7a10b01bfb"; // BeamLvl10

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        var material = node.GetMeshRenderer().material;

        material.SetColor(Color1, new Color(156f,244f,220f, 255) / 255f);
        material.SetFloat(Color1Power, 1f);
        material.SetColor(Color2, new Color(64f,176f,172f, 255) / 255f);
        material.SetFloat(BeamPower, 5);
    }
}


public class MoonLordParticles : ModDisplay
{
    private static readonly int TintColor = Shader.PropertyToID("_Color");
    
    public override string BaseDisplay => "3c445c563b7fc3f44aacf5cb2a79e424"; // BeamHitParticlesLvl10

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var renderer in node.genericRenderers)
        {
            renderer.material.SetColor(TintColor, new Color(64f,176f,172f, 255f) / 255f);
        }
    }
}
public class MoonLordBall : ModDisplay2D
{
    protected override string TextureName => "MoonlordBall";
}