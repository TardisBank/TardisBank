import * as React from 'react';
import classNames from 'classnames';
import { AppBar, Theme, IconButton, Toolbar, Typography, withStyles, WithStyles } from '@material-ui/core';
import { createStyles } from '@material-ui/core';
import { Menu } from '@material-ui/icons'

const drawerWidth = 240;

const styles = (theme: Theme) => createStyles({
    root: {
        zIndex: theme.zIndex.drawer + 1,
        transition: theme.transitions.create(['width', 'margin'], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    },
    shift: {
        marginLeft: drawerWidth,
        width: `calc(100% - ${drawerWidth}px)`,
        transition: theme.transitions.create(['width', 'margin'], {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.enteringScreen,
        }),   
    }, 
  menuButton: {
    marginLeft: 12,
    marginRight: 36,
  },
  menuButtonHidden: {
    display: 'none',
  },
  title: {
      flexGrow: 1
  }
});

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