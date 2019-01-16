import * as React from 'react';
import { WithStyles, withStyles, Theme, createStyles } from '@material-ui/core';

const styles = (theme: Theme) => createStyles({
   form: {
        width: '100%',
        marginTop: theme.spacing.unit
    },
    submit: {
        marginTop: theme.spacing.unit 
    }
});

type FormProps = {
    onSubmit: React.FormEventHandler,
    submit: React.ReactNode,
} & WithStyles<typeof styles> 

class FormBase extends React.Component<FormProps>  {

    handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        this.props.onSubmit(event);
    }

    render() {
        return (
            <form className={this.props.classes.form} onSubmit={this.handleSubmit}>
               {this.props.children} 
               <div className={this.props.classes.submit}>
                   {this.props.submit}
               </div>
            </form>
        )
    }
}

export const Form = withStyles(styles)(FormBase)