﻿using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Native.Interop;
using Avalonia.Platform;

namespace Avalonia.Native
{
    class PopupImpl : WindowBaseImpl, IPopupImpl
    {
        private readonly AvaloniaNativePlatformOptions _opts;
        private readonly AvaloniaNativeGlPlatformGraphics _glFeature;
        private readonly IWindowBaseImpl _parent;

        public PopupImpl(IAvaloniaNativeFactory factory,
            AvaloniaNativePlatformOptions opts,
            AvaloniaNativeGlPlatformGraphics glFeature,
            IWindowBaseImpl parent) : base(factory, opts, glFeature)
        {
            _opts = opts;
            _glFeature = glFeature;
            _parent = parent;
            using (var e = new PopupEvents(this))
            {
                Init(factory.CreatePopup(e, _glFeature.SharedContext.Context), factory.CreateScreens());
            }
            PopupPositioner = new ManagedPopupPositioner(new ManagedPopupPositionerPopupImplHelper(parent, MoveResize));
        }

        private void MoveResize(PixelPoint position, Size size, double scaling)
        {
            Position = position;
            Resize(size, WindowResizeReason.Layout);
            //TODO: We ignore the scaling override for now
        }

        class PopupEvents : WindowBaseEvents, IAvnWindowEvents
        {
            readonly PopupImpl _parent;

            public PopupEvents(PopupImpl parent) : base(parent)
            {
                _parent = parent;
            }

            public void GotInputWhenDisabled()
            {
                // NOP on Popup
            }

            int IAvnWindowEvents.Closing()
            {
                return true.AsComBool();
            }

            void IAvnWindowEvents.WindowStateChanged(AvnWindowState state)
            {
            }
        }

        public override void Show(bool activate, bool isDialog)
        {
            var parent = _parent;
            while (parent is PopupImpl p) 
                parent = p._parent;
            if (parent is WindowImpl w)
                w.Native.TakeFocusFromChildren();
            base.Show(false, isDialog);
        }

        public override IPopupImpl CreatePopup() => new PopupImpl(_factory, _opts, _glFeature, this);

        public void SetWindowManagerAddShadowHint(bool enabled)
        {
        }

        public IPopupPositioner PopupPositioner { get; }
    }
}
