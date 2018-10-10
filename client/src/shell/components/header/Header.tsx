import * as React from 'react';
import classNames from 'classnames';
import { AppBar, IconButton, Toolbar, Typography, withStyles, WithStyles } from '@material-ui/core';
import { Menu } from '@material-ui/icons'
import { styles } from './Header.styles';

type HeaderStateProps = {
    isOpen: boolean
}

type HeaderDispatchProps = {
    onMenuClick: () => void;
}

type HeaderProps = HeaderStateProps & HeaderDispatchProps & WithStyles<typeof styles>;

class HeaderBase extends React.Component<HeaderProps, {}> {

    render() {

        const { classes, onMenuClick } = this.props;
        return (
            <AppBar position="absolute" className={classNames(classes.root, this.props.isOpen && classes.shift)}>
                <Toolbar>
                    <IconButton 
                        color="inherit"
                        onClick={onMenuClick}
                        className={classNames(
                            classes.menuButton,
                            this.props.isOpen && classes.menuButtonHidden
                        )}
                        >
                        <Menu />
                    </IconButton>
                    <Typography 
                        variant="title" 
                        component="h1"
                        noWrap={true}
                        className={classes.title}
                        color="inherit">
                        Tardis Bank
                    </Typography>
                </Toolbar>
            </AppBar>
        );
    }
}

export const Header = withStyles(styles)(HeaderBase);