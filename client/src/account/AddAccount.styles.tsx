import { Theme, createStyles } from '@material-ui/core';
export const styles = (theme: Theme) => createStyles({
   form: {
        width: '100%',
        marginTop: theme.spacing.unit
    },
    submit: {
        marginTop: theme.spacing.unit
    }
});

