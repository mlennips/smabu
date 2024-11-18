
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { AnnualFinancialStatementDTO } from '../../types/domain/annual-financial-statement-dto';
import { getAnnualFinancialStatement, importAnnualFinancialStatementTransactions, updateAnnualFinancialStatement } from '../../services/financials.service';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { useNotification } from '../../contexts/notificationContext';
import { deepValueChange } from '../../utils/deepValueChange';
import { Grid2 as Grid, InputAdornment, List, ListItem, ListItemText, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField } from '@mui/material';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { AppError } from '../../utils/errorConverter';
import { Add, ImportExport } from '@mui/icons-material';

const AnnualFinancialStatementDetails: React.FC = () => {
    const { annualStatementId: annualFinancialStatementId } = useParams<{ annualStatementId: string }>();
    const { toast } = useNotification();
    const [data, setData] = useState<AnnualFinancialStatementDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<AppError | undefined>(undefined);
    const toolbarItems: ToolbarItem[] = [];
    const toolbarItemsIncome: ToolbarItem[] = [
        {
            text: "Importieren",
            icon: <ImportExport />,
            action: () =>
                handleAsyncTask({
                    task: () => importAnnualFinancialStatementTransactions(annualFinancialStatementId!),
                    onLoading: setLoading,
                    onSuccess: loadData,
                    onError: setError
                })
        },
        {
            text: "Neu",
            icon: <Add />,
        }
    ];

    useEffect(() => {
        loadData();
    }, [annualFinancialStatementId]);

    function loadData() {
        handleAsyncTask({
            task: () => getAnnualFinancialStatement(annualFinancialStatementId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        handleAsyncTask({
            task: () => updateAnnualFinancialStatement(annualFinancialStatementId!, {}),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Jahresabschluss erfolgreich gespeichert", "success");
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit} >
            <Stack spacing={2}>
                {renderHeaderBlock(data, loading, error, toolbarItems)}

                {renderIncomeBlock(data, handleChange, loading, toolbarItemsIncome)}

                {renderExpensesBlock(data, loading, toolbarItems)}

                <DetailsActions formId="form" deleteUrl={`/financials/annualstatements/${data?.id?.value}/delete`} disabled={loading} />
            </Stack>
        </form>
    );
};

export default AnnualFinancialStatementDetails;

const renderHeaderBlock = (data: AnnualFinancialStatementDTO | undefined, loading: boolean, error: AppError | undefined, toolbarItems: ToolbarItem[]) => {
    return <DefaultContentContainer subtitle={data?.displayName} loading={loading} error={error} toolbarItems={toolbarItems}>
        <Paper sx={{ p: 2 }}>
            <Grid container spacing={2}>
                <Grid size={{ xs: 6 }}>
                    <TextField
                        type="date"
                        fullWidth
                        id="period.from"
                        name="period.from"
                        label="Datum von"
                        value={data?.period?.from}
                        disabled />
                </Grid>
                <Grid size={{ xs: 6 }}>
                    <TextField
                        type="date"
                        fullWidth
                        id="period.to"
                        name="period.to"
                        label="Datum bis"
                        value={data?.period?.to}
                        disabled />
                </Grid>
                <Grid size={{ xs: 6, sm: 4 }}>
                    <TextField
                        type='number'
                        fullWidth
                        id="totalIncome"
                        name="totalIncome"
                        label="Einnahmen"
                        value={data?.totalIncome}
                        disabled />
                </Grid>
                <Grid size={{ xs: 6, sm: 4 }}>
                    <TextField
                        type='number'
                        fullWidth
                        id="totalExpenditure"
                        name="totalExpenditure"
                        label="Ausgaben"
                        value={data?.totalExpenditure}
                        disabled />
                </Grid>
                <Grid size={{ xs: 12, sm: 4 }}>
                    <TextField
                        type='number'
                        fullWidth
                        id="netIncome"
                        name="netIncome"
                        label="Gewinn/Verlust"
                        value={data?.netIncome}
                        disabled />
                </Grid>
            </Grid>
        </Paper>
    </DefaultContentContainer>;
}

const renderIncomeBlock = (data: AnnualFinancialStatementDTO | undefined, handleChange: (e: any) => void, loading: boolean, toolbarItems: ToolbarItem[]) => {
    return <DefaultContentContainer title={"Einnahmen"} loading={loading} toolbarItems={toolbarItems} >
        <TableContainer component={Paper}>
            <Table width="100%">
                <TableHead>
                    <TableRow>
                        <TableCell>Beschreibung</TableCell>
                        <TableCell>Import?</TableCell>
                        <TableCell>Kategorie</TableCell>
                        <TableCell align='right'>Betrag</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data?.incomes?.map((income, index) => (
                        <TableRow key={index}>
                            <TableCell component="th" scope="row">
                                <TextField
                                    fullWidth
                                    size='small'
                                    value={income.description}
                                    name={`incomes[${index}].description`}
                                    onChange={handleChange}
                                    disabled={income.isImported}
                                    variant="standard"
                                    slotProps={{ input: { disableUnderline: true } }}
                                />
                            </TableCell>
                            <TableCell>{income.isImported ? "Ja" : ""}</TableCell>
                            <TableCell>{income.category?.value}</TableCell>
                            <TableCell align='right'>
                                <TextField
                                    type="number"
                                    size='small'
                                    value={income.amount?.toFixed(2)}
                                    name={`incomes[${index}].amount`}
                                    onChange={handleChange}
                                    disabled={income.isImported}
                                    variant="standard"
                                    slotProps={{
                                        input: {
                                            disableUnderline: true,
                                            endAdornment: <InputAdornment position="end">{data?.currency?.sign}</InputAdornment>
                                         },
                                         htmlInput: { style: { textAlign: 'right' } }                        
                                    }}                    
                                />
                        </TableCell>
                        </TableRow>
                    ))}
                <TableRow key={"total"}>
                    <TableCell component="th" scope="row"><b>Summe</b></TableCell>
                    <TableCell></TableCell>
                    <TableCell></TableCell>
                    <TableCell align='right'><b>{data?.totalIncome?.toFixed(2)} {data?.currency?.sign}</b></TableCell>
                </TableRow>
            </TableBody>
        </Table>
    </TableContainer>
    </DefaultContentContainer >
}

const renderExpensesBlock = (data: AnnualFinancialStatementDTO | undefined, loading: boolean, toolbarItems: ToolbarItem[]) => {
    return <DefaultContentContainer title={"Ausgaben"} loading={loading} toolbarItems={toolbarItems} >
        <Paper sx={{ p: 2 }}>
            <List>
                {data?.expenditures?.map((expenditure, index) => (
                    <ListItem key={index}>
                        <ListItemText primary={expenditure.description} secondary={`Amount: ${expenditure.amount}`} />
                    </ListItem>
                ))}
            </List>
        </Paper>
    </DefaultContentContainer>
}

