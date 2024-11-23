
import React, { useEffect, useState } from 'react';
import { AnnualFinancialStatementDTO } from '../../types/domain/annual-financial-statement-dto';
import { getAnnualFinancialStatements } from '../../services/financials.service';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { Add, Edit, Lock, LockOpen } from '@mui/icons-material';
import { Avatar, IconButton, List, ListItem, ListItemAvatar, ListItemText } from '@mui/material';

const AnnualFinancialStatementList: React.FC = () => {
    const [data, setData] = useState<AnnualFinancialStatementDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [
        {
            text: "Neu",
            route: "/financial/annualstatements/create",
            icon: <Add />
        }
    ];

    useEffect(() => {
        handleAsyncTask({
            task: () => getAnnualFinancialStatements(),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    return (
        <DefaultContentContainer loading={loading} error={error} toolbarItems={toolbarItems}>
            <List dense={true}>
                {data.map((statement: AnnualFinancialStatementDTO) => (
                    <ListItem
                        secondaryAction={
                            <IconButton size="small" edge="end" LinkComponent="a" href={`/financial/annualstatements/${statement.id?.value}`}>
                                <Edit />
                            </IconButton>
                        }
                    >
                        <ListItemAvatar>
                            <Avatar>
                                { statement.status?.value == "Open" && <LockOpen />} 
                                { statement.status?.value != "Open" && <Lock />}

                            </Avatar>
                        </ListItemAvatar>
                        <ListItemText
                            primary={statement.fiscalYear}
                            secondary={statement.status?.value}
                        />
                    </ListItem>
                ))}
            </List>
        </DefaultContentContainer>
    );
}

export default AnnualFinancialStatementList;