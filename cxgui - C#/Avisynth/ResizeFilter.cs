namespace CXGUI.Avisynth
{
    using System;

    [Serializable]
    public enum ResizeFilter
    {
        LanczosResize,
        Lanczos4Resize,
        BicubicResize,
        BilinearResize,
        BlackmanResize,
        GaussResize,
        PointResize,
        SincResize,
        Spline16Resize,
        Spline36Resize,
        Spline64Resize,
        None
    }
}

