let keyboardHandler = null;
let dotNetReference = null;

window.setupKeyboardListener = (dotNetRef) => {
    dotNetReference = dotNetRef;
    
    keyboardHandler = (event) => {
        const key = event.key;
        
        // Handle arrow keys, space, enter, and Spotify shortcuts
        if (key === 'ArrowLeft' || key === 'ArrowRight' || key === ' ' || key === 'Enter' || 
            key === 'w' || key === 'W' || key === 'd' || key === 'D') {
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
