import { Link, useLocation } from 'react-router-dom';
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Typography from '@mui/material/Typography';
import { Box, Button, Divider, Grid, SvgIcon } from '@mui/material';
import { ArrowBack, Home as HomeIcon, Refresh } from '@mui/icons-material';
import { getItemByRoute } from '../../configs/navConfig';


const BreadcrumbsComponent: React.FC = () => {
    const location = useLocation();
    const pathnames = location.pathname.split('/').filter((x) => x);
    if (pathnames.length > 0) {
        let backTo: string | null = null;
        return (
            <Grid container size={{ xs: 12 }}>
                <Grid size={{ xs: "grow" }}>
                    <Breadcrumbs aria-label="breadcrumb"
                        sx={{ py: 1.5, fontSize: "0.85rem" }}
                        separator="/" >
                        <Link key="home" to="/">
                            <HomeIcon sx={{ fontSize: "1rem", mb: -0.2 }} />
                        </Link>
                        {pathnames.map((value, index) => {
                            const isLast = index === pathnames.length - 1;
                            const to = `/${pathnames.slice(0, index + 1).join('/')}`;
                            const navItem = getItemByRoute(to);
                            if (navItem) {
                                if (isLast) {
                                    return <Box key={to}>
                                        <Typography key="to" color="text.primary" component='span' sx={{ fontSize: "0.85rem" }}>
                                            <SvgIcon component={navItem!.icon} sx={{ mr: 0.5, fontSize: "1rem", mb: -0.3 }} />
                                            {navItem ? navItem.name : value.toUpperCase()}
                                        </Typography>
                                    </Box>

                                } else {
                                    backTo = to;
                                    return <Link key={to} to={to}>
                                        {navItem ? navItem.name : value.toUpperCase()}
                                    </Link>
                                }
                            }
                        })}
                    </Breadcrumbs>
                </Grid>
                <Grid size={{ xs: "auto" }} alignContent="center">
                    {backTo && <Button key="back" size='small' component="a" href={backTo} 
                        variant='text'
                        color='secondary'                
                        title="Zurück">
                        <ArrowBack />
                    </Button>}
                    {<Button key="refresh" size='small' component="a" href={window.location.href} 
                        variant='text'
                        color='secondary'
                        title="Neu laden">
                        <Refresh />
                    </Button>}
                </Grid>
                <Grid size={{ xs: 12 }}>
                    <Divider />
                </Grid>
            </Grid>
        );
    } else {
        return (
            <><Breadcrumbs sx={{ py: 1 }}></Breadcrumbs></>
        );
    }
};

export default BreadcrumbsComponent;