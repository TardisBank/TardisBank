import * as React from 'react';
import withStyles, { WithStyles } from '@material-ui/core/styles/withStyles';
import { Theme, createStyles, Dialog, DialogContent, DialogActions, Button, Typography, DialogContentText } from '@material-ui/core';
import { withRoot } from '../withRoot'

type State = {
    open: boolean;
}

const styles = (theme: Theme) => 
    createStyles({
        root: {
            textAlign: 'center',
            paddingTop: theme.spacing.unit * 20
        }
    });


class IndexBase extends React.Component<WithStyles<typeof styles>, State> {
   
    state = {
        open: false
    };

    handleClose = () => {
        this.setState({
            open: false
        });
    };

    handleClick = () => {
        this.setState({
            open: true 
        });
    };
    
    render() {
        return (
            <div className={this.props.classes.root}>
                <Dialog 
                    open={this.state.open}
                    onClose={this.handleClose}>
                    <DialogContent>
                        <DialogContentText>Hello there</DialogContentText>
                    </DialogContent>
                    <DialogActions>
                        <Button 
                            color="primary"
                            onClick={this.handleClose}>
                            OK
                        </Button>
                    </DialogActions>
                </Dialog>
                <Typography variant="display1" gutterBottom={true}>
                    Material-UI
                </Typography>
                <Typography variant="subheading" gutterBottom={true}>
                    example project 
                </Typography>
                <Button variant="raised" color="secondary" onClick={this.handleClick}>
                    Press me!
                </Button>
            </div>
        )
    }
}

export const Index = withRoot(withStyles(styles)(IndexBase))