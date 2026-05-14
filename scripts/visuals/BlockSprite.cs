using Godot;

[GlobalClass]
public partial class BlockSprite : Sprite2D, IBlockSprite
{
    [Export] Sprite2D Layer2;


    public void SetTexture(Rect2 rect)
    {
        RegionRect = rect;
    }

    public void SetTexture(Texture2D texture, Rect2 region)
    {

        Texture = texture;
        RegionRect = region;
    }

    public void SetSecondLayerTexture(Rect2 region)
    {

        Layer2.RegionRect = region;
    }
    public void SetSecondLayerTexture(Texture2D texture, Rect2 region)
    {

        Layer2.Texture = texture;
        Layer2.RegionRect = region;
    }

    public void SetColor(float hue, float sat, float val)
    {

        SelfModulate = Color.FromHsv(hue, sat, val);

        Layer2.SelfModulate = Utils.GetSecondLayerColor(hue, sat, val);
    }

    public void ToggleVisibility(bool visible)
    {
        Visible = visible;
    }

   
}
