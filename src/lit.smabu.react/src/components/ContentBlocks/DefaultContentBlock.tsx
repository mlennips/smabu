import { Box, Button, ButtonGroup, LinearProgress, Toolbar, Typography } from '@mui/material';
import React from 'react';
import { getItemByCurrentLocation } from '../../configs/navConfig';
import { blueGrey, grey } from '@mui/material/colors';
import { AppError } from '../../utils/errorConverter';
import ErrorComponent from '../ContentBlocks/ErrorComponent';

interface DefaultContentContainerProps {
    title?: string | undefined | null;
    subtitle?: string | undefined | null;
    children?: React.ReactNode;
    loading?: boolean;
    error?: AppError | undefined | null;
    toolbarItems?: ToolbarItem[] | undefined;
}

export interface ToolbarItem {
    text: string;
    icon?: React.ReactNode;
    route?: string,
    action?: () => void;
    showMode?: "onlyText" | "onlyIcon" | "both";
    color?: 'inherit' | 'primary' | 'secondary' | 'success' | 'error' | 'info' | 'warning';
    title?: string;
}

const DefaultContentContainer: React.FC<DefaultContentContainerProps> = ({ title, subtitle, children, loading, error, toolbarItems }) => {
    if (title === null || title === undefined) {
        const item = getItemByCurrentLocation();
        title = item?.name;
    }

    return (
        <Box component="section">
            <Toolbar
                variant="dense"
                sx={[
                    {
                        pl: { sm: 0 },
                        pr: { xs: 1, sm: 1 },
                        py: 0
                    },
                ]}>
                <Typography component="h1">
                    {title && title !== undefined && <Typography variant="h6" component="span" sx={{ color: (subtitle ? grey[300] : blueGrey[500]), fontWeight: "bold" }}>{title}</Typography>}
                    {subtitle && subtitle !== undefined && <Typography variant="h6" component="span" sx={{ color: blueGrey[500], ml: 0.5, fontWeight: "bold" }}>{subtitle}</Typography>}
                </Typography>
                <Box sx={{ flex: 1 }}>

                </Box>
                <ButtonGroup>
                    {toolbarItems && toolbarItems.map((item, index) => {
                        item.showMode === undefined ? item.showMode = "both" : item.showMode;
                        return (
                            <Button key={index} size='small' variant="text" startIcon={item.icon}
                                disabled={loading}
                                color={item.color}
                                onClick={item.action ? () => item.action!() : undefined}
                                component={item.route ? "a" : "button"}
                                href={item.route ? item.route : undefined}
                                title={item.title ?? item.text}>
                                {item.showMode == "onlyText" || item.showMode == "both" && item.text}
                            </Button>
                        );
                    })}
                </ButtonGroup>
            </Toolbar>
            {error && ErrorComponent(error)}
            {!loading && children}
            {loading && !error && <Box sx={{ opacity: 0.2 }}>
                {children}
                <LinearProgress sx={{ mt: -0.5 }} />
            </Box>}
        </Box>
    );
};

export default DefaultContentContainer;