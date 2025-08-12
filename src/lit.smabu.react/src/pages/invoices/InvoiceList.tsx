import { useEffect, useState } from "react";
import DefaultContentContainer, { ToolbarItem } from "../../components/contentBlocks/DefaultContentBlock";
import { Add, Delete, Edit } from "@mui/icons-material";
import { DataGrid, GridActionsCellItem, GridColDef } from '@mui/x-data-grid';
import { formatDate } from "../../utils/formatDate";
import { getInvoices } from "../../services/invoice.service";
import { Link } from "react-router-dom";
import { Paper } from "@mui/material";
import { handleAsyncTask } from "../../utils/handleAsyncTask";
import { ListInvoicesDTO } from "../../types/domain/list-invoices-dto";

const columns: GridColDef[] = [
    { field: 'number', headerName: '#', width: 120, valueGetter: (value: any) => value.displayName },
    { field: 'fiscalYear', headerName: 'Jahr', width: 70 },
    { field: 'createdAt', headerName: 'Erstellt am', width: 100, valueFormatter: (value) => formatDate(value) },
    { field: 'customer', headerName: 'Kunde', flex: 1, valueGetter: (value: any) => value.name },
    { field: 'amount', headerName: 'Summe', width: 110, align: 'right', valueFormatter: (value: any, row) => `${value.toFixed(2)} ${row.currency?.isoCode}` },
    { field: 'releasedAt', headerName: 'Freigegeben am', width: 100, valueFormatter: (value) => formatDate(value) },
    { field: 'isPaid', headerName: 'Beglichen', width: 100, type: 'boolean' },
    {
        field: 'actions',
        type: 'actions',
        headerName: '',
        width: 100,
        getActions: ({ id }) => {
            return [
                <GridActionsCellItem
                    icon={<Edit />}
                    label="Öffnen"
                    className="textPrimary"
                    component={Link}
                    onClick={() => window.location.href = `/invoices/${id}`}
                    color="primary"
                />,
                <GridActionsCellItem
                    icon={<Delete />}
                    label="Delete"
                    component={Link}
                    onClick={() => window.location.href = `/invoices/${id}/delete`}
                />,
            ];
        },
    }
];

const paginationModel = { page: 0, pageSize: 10 };

const InvoiceList = () => {
    const [data, setData] = useState<ListInvoicesDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [
        {
            text: "Neu",
            route: "/invoices/create",
            icon: <Add />
        }
    ];

    useEffect(() => {
        handleAsyncTask({
            task: getInvoices,
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    return (
        <DefaultContentContainer loading={loading} error={error} toolbarItems={toolbarItems}>
            <Paper sx={{ flex: 1, overflow: 'hidden' }}>
                <DataGrid
                    rows={data}
                    columns={columns}
                    getRowId={(row) => row.id.value}
                    isRowSelectable={() => false}
                    initialState={{ pagination: { paginationModel } }}
                    pageSizeOptions={[10, 50, 100]}
                    sx={{ border: 0 }}
                />
            </Paper>
        </DefaultContentContainer>
    );
}

export default InvoiceList;