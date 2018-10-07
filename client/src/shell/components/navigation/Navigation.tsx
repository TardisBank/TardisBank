import * as React from 'react';
import classNames from 'classnames';
import { Drawer, List, ListItem, ListItemText, withStyles, WithStyles, ListItemIcon, IconButton } from '@material-ui/core';
import { CreditCard, People, CalendarToday, ChevronLeft } from '@material-ui/icons';
import { Theme, createStyles } from '@material-ui/core';

const drawerWidth = 240;

const styles = (theme: Theme) => createStyles({
    drawerPaper: {
        position: 'relative',
        whiteSpace: 'nowrap',
        width: drawerWidth,
        transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen
        })
    },
    drawerPaperClose: {
        overflowX: 'hidden',
        transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
        width: theme.spacing.unit * 7,
        [theme.breakpoints.up('sm')]: {
            width: theme.spacing.unit * 9,
        },
    },
    toolbarIcon: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'flex-end',
        padding: '0 8px',
        ...theme.mixins.toolbar,
    },
});

type NavigationStateProps = {
    isOpen: boolean
}

type NavigationDispatchProps = {
    onToobarIconClick: () => void
}

type NavigationProps = NavigationStateProps & NavigationDispatchProps & WithStyles<typeof styles>

class nav extends React.Component<NavigationProps, {}> {

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
                <List>
                    <ListItem button={true}>
                        <ListItemIcon>
                            <People />
                        </ListItemIcon>
                        <ListItemText primary="Children" />
                    </ListItem>
                    <ListItem button={true}>
                        <ListItemIcon>
                            <CreditCard />
                        </ListItemIcon>
                        <ListItemText primary="Transactions" />
                    </ListItem>
                    <ListItem button={true}>
                        <ListItemIcon>
                            <CalendarToday />
                        </ListItemIcon>
                        <ListItemText primary="Schedules" />
                    </ListItem>
                </List>
            </Drawer>
        );
    }
}

export const Navigation = withStyles(styles)(nav);