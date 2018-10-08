import * as React from 'react';
import {MuiThemeProvider, createMuiTheme, CssBaseline} from '@material-ui/core';
import green from '@material-ui/core/colors/green';

const theme = createMuiTheme({
    palette: {
        secondary: green
    }
});

export function withRoot<P>(Component: React.ComponentType<P>) {
    function withRoot(props: P) {
        return (
            <MuiThemeProvider theme={theme}>
                <CssBaseline />
                <Component {...props} />
            </MuiThemeProvider>
        )
    }

    return withRoot;
}
