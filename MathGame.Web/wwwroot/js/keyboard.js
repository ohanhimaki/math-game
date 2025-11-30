let keyboardHandler = null;
let dotNetReference = null;

window.setupKeyboardListener = (dotNetRef) => {
    dotNetReference = dotNetRef;
    
    keyboardHandler = (event) => {
        const key = event.key;
        
        // Don't intercept if user is typing in an input or textarea
        const target = event.target;
        if (target && (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA')) {
            return; // Let the input handle the key
        }
        
        // Handle arrow keys, space, enter, number keys 0-9, and Spotify shortcuts
        if (key === 'ArrowLeft' || key === 'ArrowRight' || key === ' ' || key === 'Enter' || 
            key === 'w' || key === 'W' || key === 'd' || key === 'D' ||
            (key >= '0' && key <= '9')) {
            event.preventDefault();
            
            if (dotNetReference) {
                dotNetReference.invokeMethodAsync('HandleKeyPress', key);
            }
        }
    };
    
    document.addEventListener('keydown', keyboardHandler);
};

window.removeKeyboardListener = () => {
    if (keyboardHandler) {
        document.removeEventListener('keydown', keyboardHandler);
        keyboardHandler = null;
    }
    dotNetReference = null;
};
