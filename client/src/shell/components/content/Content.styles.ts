import { Theme, createStyles } from '@material-ui/core';
export const styles = (theme: Theme) => createStyles({
   paper: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'left',
        padding: `${theme.spacing.unit * 2}px ${theme.spacing.unit * 3}px ${theme.spacing.unit * 3}px`

    }
});

