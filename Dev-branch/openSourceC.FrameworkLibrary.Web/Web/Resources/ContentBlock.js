var oscContentBlock = function() {
    return {
        toggleContent: function(divId, imageId) {
            var contentDiv = document.getElementById(divId);
            var image = document.getElementById(imageId);

            if (contentDiv.style.display == 'none') {
                contentDiv.style.display = 'block';
                image.className = 'osc-ContentBlock-exp';
            }
            else {
                contentDiv.style.display = 'none';
                image.className = 'osc-ContentBlock-col';
            }
        },
        disableTextSelection: function(e) {
            return false;
        },
        enableTextSelection: function() {
            return true;
        },
        handleMouseUp: function(e) {
            if (typeof (e) == 'undefined')
                e = window.event || window.Event;

            var mX = null;
            var mY = null;
            modifiers = 0;

            if (typeof (e.offsetX) != 'undefined') {
                mX = e.offsetX;
                mY = e.offsetY;
            }
            else if (typeof (e.layerX) != 'undefined') {
                mX = e.layerX;
                mY = e.layerY;
            }

            if (typeof (e.ctrlKey) != 'undefined') {
                if (e.ctrlKey)
                    modifiers |= 1;
                if (e.shiftKey)
                    modifiers |= 2;
            }
            else {
                if (e.modifiers & Event.CONTROL_MASK)
                    modifiers |= 1;

                if (e.modifiers & Event.SHIFT_MASK)
                    modifiers |= 2;
            }
        }
    };
} ();