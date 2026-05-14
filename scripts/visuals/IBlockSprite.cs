

using Godot;

public interface IBlockSprite
{
    public void SetTexture(Rect2 region);
    public void SetTexture(Texture2D texture, Rect2 region);
    public void SetSecondLayerTexture(Rect2 region);
    public void SetSecondLayerTexture(Texture2D texture, Rect2 region);

    public void SetColor(float hue, float sat, float val);

    public void Rotate(float radians);
    public void ToggleVisibility(bool visible);
}