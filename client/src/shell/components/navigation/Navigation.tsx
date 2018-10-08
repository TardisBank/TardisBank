import * as React from 'react';
import { Drawer, List, ListItem, ListItemText, withStyles, WithStyles, ListItemIcon } from '@material-ui/core';
import { CreditCard, People, CalendarToday } from '@material-ui/icons';
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
    } 
});

type NavigationProps = WithStyles<typeof styles>

class nav extends React.Component<NavigationProps,{}> {

    render() {

        const {classes} = this.props;

        return (
            <Drawer 
                variant="permanent"
                open={true}
                classes={{
                    paper: classes.drawerPaper 
                }}>
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