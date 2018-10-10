import { createStyles, Theme } from '@material-ui/core';

export const styles = (theme: Theme) => createStyles({
    root: {
        display: 'flex',
    },
    content: {
        flexGrow: 1,
        padding: theme.spacing.unit * 3,
        height: '100vh',
        overflow: 'auto'
    },
    appBarSpacer: theme.mixins.toolbar
});

