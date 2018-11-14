import * as React from 'react';
import classNames from 'classnames';
import { Drawer, withStyles, WithStyles, IconButton, Button } from '@material-ui/core';
import { ChevronLeft, AddCircle } from '@material-ui/icons';
import { styles } from './Navigation.styles';

type NavigationStateProps = {
    isOpen: boolean,
}

type NavigationDispatchProps = {
    onToobarIconClick: () => void,
    onAddAccount: () => void;
}

type NavigationProps = NavigationStateProps & NavigationDispatchProps & WithStyles<typeof styles>

class navigation extends React.Component<NavigationProps, {}> {

    onAddAccountClick = () => this.props.onAddAccount();

    render() {

        const { classes, onToobarIconClick } = this.props;

        return (
            <Drawer
                variant="permanent"
                open={true}
                classes={{
                    paper: classNames(classes.drawerPaper, !this.props.isOpen && classes.drawerPaperClose)
                }}>
                <div className={classes.toolbarIcon}>
                    <IconButton onClick={onToobarIconClick}>
                        <ChevronLeft />
                    </IconButton>
                </div>
                {this.props.children}
                <Button
                    onClick={this.onAddAccountClick}>
                    <AddCircle fontSize="large" />

                </Button>
            </Drawer>
        );
    }
}

export const Navigation = withStyles(styles)(navigation);