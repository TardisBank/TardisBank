import * as React from 'react';
import { AppBar, IconButton, Toolbar, Typography, withStyles, WithStyles } from '@material-ui/core';
import { createStyles } from '@material-ui/core';

const styles = () => createStyles({
    root: {
        flexGrow: 1
    },
    grow: {
        flexGrow: 1
    },
    menuButton: {
        marginLeft: -12,
        marginRight: 20
    }
});
type HeaderProps = WithStyles<typeof styles>;

class HeaderBase extends React.Component<HeaderProps, {}> {

    render() {

        const { classes } = this.props;

        return (
            <div className={classes.root}>
                <AppBar position="static">
                    <Toolbar>
                        <IconButton className={classes.menuButton} color="inherit">
                            <span>M</span>
                        </IconButton>
                        <Typography variant="title" color="inherit" className={classes.grow}>
                            Tardis Bank
                        </Typography>
                    </Toolbar>
                </AppBar>
            </div>
        );
    }
}

export const Header = withStyles(styles)(HeaderBase);