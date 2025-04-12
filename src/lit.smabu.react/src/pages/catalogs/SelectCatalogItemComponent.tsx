import { ContentPasteGo } from '@mui/icons-material';
import { Grid, Autocomplete, TextField, ButtonGroup, Button } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { getCatalogItems } from '../../services/catalogs.service';
import { CatalogItemDTO, CatalogItemId, CustomerId, Unit } from '../../types/domain';

interface SelectCatalogItemComponentProps {
    customerId: CustomerId,
    getCatalogItemId: () => CatalogItemId | undefined | null,
    setCatalogItemId: (id: CatalogItemId | undefined) => void,
    setDetails?: (value: string) => void | null,
    setPrice?: (value: number) => void | null,
    setUnit?: (value: Unit) => void | null,
}

const SelectCatalogItemComponent: React.FC<SelectCatalogItemComponentProps> = ({ customerId, getCatalogItemId, setCatalogItemId, setDetails, setPrice, setUnit }) => {

    const [catalogItems, setCatalogItems] = useState<CatalogItemDTO[]>([]);
    const selectedCatalogItemId = getCatalogItemId();
    const selectedValue = React.useMemo(() =>
        catalogItems.find((item: any) => getCatalogItemId() && item.id?.value === getCatalogItemId()?.value),
        [catalogItems, selectedCatalogItemId]);

    useEffect(() => {
        getCatalogItems().then(items => setCatalogItems(items));
    }, []);

    const setValuesFromCatalog = (mode: 'details' | 'price' | 'unit') => {
        const catalogItemId = getCatalogItemId();
        const catalogItem = catalogItems.find(x => x.id?.value == catalogItemId?.value);
        switch (mode) {
            case 'details':;
                if (setDetails != null) { setDetails(catalogItem?.name?.toUpperCase() + (catalogItem?.description ? ": " + catalogItem?.description : "")) };
                break;
            case 'price':
                const price = catalogItem?.customerPrices?.find(x => x.customerId?.value == customerId.value) ?? catalogItem?.currentPrice;
                if (setPrice != null) { setPrice(price?.price ?? 0) };
                break;
            case 'unit':
                if (setUnit != null) { setUnit(catalogItem?.unit!) };
                break;
        }
    }

    return (
        <>
            {catalogItems?.length > 0 && <Grid container>
                <Grid size={{ xs: 12, sm: "grow", md: "grow" }}>
                    <Autocomplete
                        fullWidth
                        isOptionEqualToValue={(option, value) => option?.id?.value === value?.id?.value}
                        getOptionLabel={(option) => option?.displayName!}
                        getOptionKey={(option) => option.id?.value!}
                        getOptionDisabled={(option) => !option.isActive}
                        value={selectedValue}
                        onChange={(_e, value) => setCatalogItemId(value?.id)}
                        groupBy={(option) => option.groupName ?? ""}
                        options={catalogItems}
                        autoHighlight
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label="Katalog durchsuchen"
                            />
                        )} />
                </Grid>
                <Grid size={{ xs: 12, sm: "auto", md: "auto" }} container alignItems="flex-end">
                    {getCatalogItemId() && <ButtonGroup
                        size='small'
                        variant='text'
                    >
                        <Button title='Details übernehmen' disabled={!setDetails}
                            startIcon={<ContentPasteGo />}
                            onClick={() => setValuesFromCatalog('details')}>
                            Beschreibung
                        </Button>
                        <Button title='Preis übernehmen' disabled={!setPrice}
                            startIcon={<ContentPasteGo />}
                            onClick={() => setValuesFromCatalog('price')}>
                            Preis
                        </Button>
                        <Button title='Einheit übernehmen' disabled={!setUnit}
                            startIcon={<ContentPasteGo />}
                            onClick={() => setValuesFromCatalog('unit')}>
                            Einheit
                        </Button>
                    </ButtonGroup>}
                </Grid>
            </Grid>}
            {catalogItems?.length === 0 && <Autocomplete
                fullWidth
                disablePortal
                options={[]}
                loading={true}
                renderInput={(params) => <TextField {...params} />} />
            }
        </>
    );
};

export default SelectCatalogItemComponent;