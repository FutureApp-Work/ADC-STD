# ADC-STD Backend Rewrite - Master Specification

## Executive Summary

This document provides a comprehensive specification for rewriting the ADC-STD backend from Python to C# .NET, consolidating multiple applications into a unified system.

### Current Architecture
- **app001** (baseURL_P): Patient, Login, Permission, Settings, Take (取藥)
- **app002** (baseURL_S): Inventory/Count (盤點), Cabinet, Unit, Common Amount, Borrow
- **app003** (baseURL_L): Return (退藥), Recycle/Recall (回收), Replenish/Supply (撥補), Standing (常備量)
- **appReport**: Reports and queries
- **appScheduler**: Background tasks
- **imadc**: Hardware control (serial port communication)

### Target Architecture

Unified C# .NET application with:
- Single WebAPI host combining all services
- Internal hardware control module (no separate API)
- MariaDB database (migrating from PostgreSQL)
- Preserved URL routes for all frontend compatibility

---

## 1. API Endpoint Inventory

### 1.1 Authentication & User Management

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/login` | POST | app001/appReport | User login |
| `/loginUseAccountOnly` | POST | app001/appReport | SSO login |
| `/doubleVerification` | POST | app001 | Two-factor auth |
| `/changePassword` | POST | app001/appReport | Change password |
| `/getUserList` | GET | app001 | List users |
| `/getUser` | GET | app001/appReport | Get user details |
| `/createUser` | POST | app001/appReport | Create user |
| `/updateUser` | POST | app001/appReport | Update user |
| `/deleteUser` / `/deleteUser/{id}` | DELETE | app001/appReport | Delete user |
| `/getRoleList` | GET | app001/appReport | List roles |
| `/getPermissionList` | GET | app001 | List permissions |
| `/importUserCsv` | POST | app001 | Import users |
| `/getImage/{id}` | GET | appReport | Get user photo |

### 1.2 Settings & Configuration

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/getSetting` | GET | app001 | Get system settings |
| `/setting_api/get_MasterCabinetInfo` | GET | app002 | Master cabinet info |
| `/setting_api/set_Cabinet` | POST | app002 | Update cabinet |
| `/setting_api/get_SlaveCabinetInfo` | GET | app002 | Slave cabinet info |
| `/setting_api/get_dropList` | POST | app002/appReport | Dropdown lists |

### 1.3 Patient & Medication Operations (取藥)

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/getPatientList` | GET | app001 | List patients |
| `/getPrescriptionDetail` | GET | app001 | Prescription details |
| `/getStationList` | GET | app001 | Station list |
| `/createPrescriptionGetTask` | POST | app001 | Create prescription task |
| `/createMedicationGetTask` | POST | app001 | Create medication task |
| `/currentGetStorage` | GET | app001 | Current storage |
| `/openBox` | GET | app001 | Open box |
| `/getAction` | POST | app001 | Get action history |
| `/boxList` | GET | app001 | List boxes |
| `/checkBox` | GET | app001 | Check box status |
| `/boxInfo` | GET | app001 | Box info |
| `/forceInventory` | POST | app001 | Force inventory |
| `/cumulativeGetMedicationKnowledgeBoxList` | GET | app001 | Cumulative get box list |
| `/cumulativeGetPrescriptionDetailList` | GET | app001 | Cumulative prescription list |
| `/createCumulativeGetTask` | POST | app001 | Create cumulative task |
| `/cumulativeGetCurrentStorage` | GET | app001 | Cumulative storage |
| `/getBindPrescriptionDetail` | GET | app001 | Bind prescription detail |
| `/getBindGetActionHistoryList` | GET | app001 | Bind action history list |
| `/getBindGetActionHistory` | GET | app001 | Bind action history |
| `/getBindPrescriptionDetailCompareGetActionHistory` | GET | app001 | Compare history |
| `/bindPrescriptionAndMedicationGet` | POST | app001 | Bind prescription & get |
| `/getBlindInventory` | GET | app001 | Blind inventory |
| `/updateDailyAverageAmountDescriptionByTask` | POST | app001 | Update description |
| `/getBindPrescriptionList` | GET | app001 | Bind prescription list |

### 1.4 Return Operations (退藥)

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/Return_api/creatReturnMediaction` | POST | app003 | Create return |
| `/Return_api/reconsider_open_box` | POST | app003 | Reconsider open box |
| `/Return_api/returnMedicationNextBox` | POST | app003 | Next box |
| `/Return_api/returnMediactionfinish` | POST | app003 | Finish return |
| `/Return_api/findReturnMedication` | POST | app003 | Find return |
| `/Return_api/returnMedication_check_lock` | POST | app003 | Check lock |
| `/Return_api/getReturnPage` | POST | app003 | Get return page |
| `/Return_api/creatNoPrescriptionReturntask` | POST | app003 | No prescription return |
| `/Return_api/noPrescriptionReturnFinish` | POST | app003 | Finish no prescription |
| `/Return_api/findPrescriptionMedication` | GET | app003 | Find prescription |
| `/Return_api/noPrescriptionReturnAbolish` | POST | app003 | Abolish return |
| `/Refill_api/create_Refill_task` | POST | app003 | Create refill task |
| `/Refill_api/create_Refill_task_storage` | POST | app003 | Refill storage |
| `/Refill_api/get_Task_detail` | GET | app003 | Task detail |
| `/Refill_api/Refill_box_check` | GET | app003 | Box check |

### 1.5 Recall/Recycle Operations (回收)

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/Recall/recall_task` | POST | app003 | Recall task |
| `/Recall/get_recall_box_list` | GET | app003 | Recall box list |
| `/Recall/recall_box_info` | POST | app003 | Recall box info |
| `/Recall/recall_open_box` | POST | app003 | Recall open box |
| `/Recall/recall_in_DB` | POST | app003 | Recall to DB |
| `/Recall/recall_Mediaction` | GET | app003 | Recall medication |
| `/Recall/get_recall_box_info` | POST | app003 | Get box info |
| `/Recall/recall_box_task_done` | POST | app003 | Task done |
| `/Recall/recall_box_stock_delete` | POST | app003 | Delete stock |
| `/Recall/doubleVerification` | POST | app003 | Double verification |
| `/Recall/get_recall_box_info_list` | GET | app003 | Box info list |
| `/Recall/clean_recall_task` | POST | app003 | Clean task |
| `/Recall/recall_Mediaction_no_prescription` | GET | app003 | No prescription recall |
| `/Recall/create_blindinventory_history` | POST | app003 | Blind inventory history |
| `/Recall/Recall_box_check` | GET | app003 | Box check |
| `/Recall/get_unbound_recall` | GET | app003 | Unbound recall |
| `/Recall/get_uabound_get_list` | GET | app003 | Unbound get list |
| `/Recall/Recall_connect_recall_and_get` | POST | app003 | Connect recall & get |
| `/Recall/recall_box_stock_delete_have_next` | POST | app003 | Delete with next |

### 1.6 Supply/Replenish Operations (撥補)

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/Supply_api/get_cabinet_low_inventory` | POST | app003 | Low inventory |
| `/Supply_api/supply_task` | POST | app003 | Supply task |
| `/Supply_api/reconsider_open_box` | POST | app003 | Reconsider box |
| `/Supply_api/supply_close` | GET | app003 | Supply close |
| `/Supply_api/supply_in_DB` | POST | app003 | Supply to DB |
| `/Supply_api/Drawwer_task` | POST | app003 | Drawer task |
| `/Supply_api/get_medication_batch_expirationDate` | GET | app003 | Batch expiration |
| `/Supply_api/Mission_Discontinue` | POST | app003 | Mission discontinue |
| `/Supply_api/supply_Drawer_finsh` | POST | app003 | Drawer finish |
| `/Supply_api/supply_drawer_info` | GET | app003 | Drawer info |
| `/Supply_api/Drawer_task_openbox` | POST | app003 | Open box |
| `/Supply_api/supply_checkdrawer` | GET | app003 | Check drawer |
| `/Supply_api/get_supply_source` | GET | app003 | Supply source |
| `/Supply_api/get_empty_bottle_supply_list` | GET | app003 | Empty bottle list |
| `/Supply_api/supply_medication_find_box` | GET | app003 | Find box |

### 1.7 Inventory/Count Operations (盤點)

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/inventory_api/get_cabinet` | GET | app002 | Get cabinets |
| `/inventory_api/get_drug_info` | POST/GET | app002 | Drug info |
| `/inventory_api/Create_DrawerBox_Task` | POST | app002 | Create drawer task |
| `/inventory_api/Create_CabinetDrawer_Task` | POST | app002 | Cabinet drawer task |
| `/inventory_api/auto_openBox` | GET | app002 | Auto open box |
| `/inventory_api/reopenDrawer` | GET | app002 | Reopen drawer |
| `/inventory_api/getStatusRequest` | POST | app002 | Status request |
| `/inventory_api/checkDrawerStatus` | GET | app002 | Check drawer status |
| `/inventory_api/inventory` | POST | app002 | Inventory count |
| `/inventory_api/abnormal_recode` | POST | app002 | Abnormal record |
| `/inventory_api/get_drawer` | GET | app002 | Get drawer |
| `/inventory_api/get_Drawer_detail` | POST | app002 | Drawer detail |
| `/inventory_api/select_box` | POST | app002 | Select box |
| `/inventory_api/skip_Drawer` | POST | app002 | Skip drawer |
| `/inventory_api/goback` | GET | app002 | Go back |
| `/inventory_api/get_drug_boxInfo` | GET | app002 | Drug box info |
| `/inventory_api/get_drug_Type_and_Level` | GET | app002 | Type and level |
| `/inventory_api/getDrugError` | GET | app002 | Drug error |
| `/inventory_api/inventory_double_verification` | POST | app002 | Double verification |
| `/inventory_api/update_task_status` | POST | app002 | Update task status |
| `/inventory_api/get_empty_bottle_inventory_medicine_list` | GET | app002 | Empty bottle list |
| `/inventory_api/create_empty_bottle_inventory_task` | POST | app002 | Create empty bottle task |
| `/inventory_api/get_empty_bottle_medicine_drawer_box_list` | GET | app002 | Empty bottle boxes |
| `/inventory_api/empty_bottle_inventory_open_box` | POST | app002 | Empty bottle open box |
| `/inventory_api/update_empty_bottle_inventory_task_status` | POST | app002 | Update empty bottle status |
| `/inventory_api/inventory_create_blind_inventory_history` | POST | app002 | Blind inventory history |
| `/verification_api/check_get_medication_list` | GET | app002 | Check medication list |
| `/verification_api/check_inventory` | POST | app002 | Check inventory |
| `/verification_api/get_check_question_list` | GET | app002 | Question list |
| `/verification_api/verify` | POST | app002 | Verify |
| `/verification_api/create_inventory_check_history` | POST | app002 | Check history |
| `/verification_api/check_inventory_double_verification` | POST | app002 | Double verification |
| `/verification_api/delete_storage_detail` | DELETE | app002 | Delete storage |
| `/inventory_api/get_alter_flag` | GET | app002 | Alter flag |

### 1.8 Standing/Common Amount Operations (常備量)

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/callout_api/common_amount_callout_task` | POST | app003 | Callout task |
| `/callout_api/common_amount_callout_task_cancel` | POST | app003 | Cancel task |
| `/callout_api/common_task_box_list` | POST | app003 | Task box list |
| `/callout_api/common_amount_callout_task_open_box` | POST | app003 | Open box |
| `/callout_api/common_amount_callout_task_write` | POST | app003 | Write task |
| `/common_amount_api/get_common_amount_form` | GET | app002 | Get form |
| `/common_amount_api/get_common_amount_status_list` | GET | app002/appReport | Status list |
| `/common_amount_api/get_common_amount_form_detail` | GET | app002 | Form detail |
| `/common_amount_api/review_common_amount_form` | POST | app002 | Review form |
| `/appReport/common_amount/create_common_amount_form` | POST | appReport | Create form |
| `/appReport/common_amount/get_medication` | GET | appReport | Get medication |
| `/appReport/common_amount/get_common_amount_form_list` | GET | appReport | Form list |

### 1.9 Borrow Operations (借藥)

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/borrow_medication_api/get_borrow_medication_form` | GET | app002 | Get form |
| `/borrow_medication_api/get_medication` | GET | app002 | Get medication |
| `/borrow_medication_api/create_borrow_medication_form` | POST | app002 | Create form |
| `/borrow_medication_api/get_borrow_medication_form_detail` | GET | app002 | Form detail |
| `/borrow_medication_api/get_borrow_medication_form_notify` | GET | app002 | Form notify |
| `/borrow_medication_api/create_borrow_medication_form_notify_on_read` | POST | app002 | Mark read |
| `/borrow_medication_api/delete_borrow_medication_form` | DELETE | app002 | Delete form |
| `/borrow_medication_api/update_borrow_medication_form` | POST | app002 | Update form |
| `/appReport/borrow_medication/get_borrow_medication_form_list` | GET | appReport | Form list |
| `/appReport/borrow_medication/review_borrow_medication_form` | POST | appReport | Review form |
| `/appReport/borrow_medication/get_borrow_medication_form_history` | GET | appReport | Form history |
| `/appReport/borrow_medication/return_medication` | POST | appReport | Return medication |

### 1.10 Cabinet/Drawer/Box Management

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/setting_api/get_DrawerBox_list` | POST | app002 | Drawer/box list |
| `/setting_api/get_drawerBoxDetail` | GET | app002 | Box detail |
| `/setting_api/setDrawerBox` | POST | app002 | Set box |
| `/setting_api/update_drawerBox` | POST | app002 | Update box |
| `/setting_api/delete_storage` | DELETE | app002 | Delete storage |
| `/setting_api/getDrawer_and_Box` | GET | app002 | Drawers and boxes |
| `/setting_api/get_boxMaintenanceHistoryList` | POST | app002 | Box maintenance |
| `/setting_api/get_drawerMaintenanceHistoryList` | POST | app002 | Drawer maintenance |
| `/setting_api/maintenanceElectronicLock` | POST | app002 | Lock maintenance |

### 1.11 Medication Management

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/setting_api/get_Medication` | POST | app002 | Get medications |
| `/setting_api/getMedicationName` | GET | app002 | Search names |
| `/setting_api/update_Medication` | POST | app002 | Update medication |
| `/setting_api/add_Medication` | POST | app002 | Add medication |
| `/setting_api/get_MedicationDetail` | GET | app002 | Medication detail |
| `/setting_api/add_Medication_CSV` | POST | app002 | Import CSV |
| `/setting_api/del_Medication` | POST | app002 | Delete medication |
| `/setting_api/get_medication_excel` | GET | app002 | Export Excel |
| `/appReport/getMedicationKnowledgeCodeList` | GET | appReport | Knowledge codes |

### 1.12 Notification & Maintenance

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/setting_api/get_Informer` | POST | app002 | Notification list |
| `/setting_api/seach_Informer` | GET | app002 | Search notification |
| `/setting_api/add_Informer` | POST | app002 | Add notification |
| `/setting_api/del_Informer` | POST | app002 | Delete notification |
| `/setting_api/get_supply_source_list` | GET | app002 | Supply source list |
| `/setting_api/create_supply_source` | POST | app002 | Create source |
| `/setting_api/update_supply_source` | POST | app002 | Update source |
| `/setting_api/delete_supply_source` | DELETE | app002 | Delete source |

### 1.13 Report & Query APIs

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/appReport/Reportstorage_api/get_findMediaction` | POST | appReport | Inventory query |
| `/appReport/Reportstorage_api/get_findMediaction_excel` | GET | appReport | Export inventory |
| `/appReport/Reportstorage_api/get_all_cabinet_stock` | POST | appReport | All cabinet stock |
| `/appReport/Reportstorage_api/export_all_cabinet_stock` | POST | appReport | Export stock |
| `/appReport/Reportstorage_api/get_low_medication_list` | POST | appReport | Low medication |
| `/appReport/Reportstorage_api/getLowMedicationListReport` | POST | appReport | Export low med |
| `/appReport/Reportstorage_api/get_findCloseExpiryMediaction` | POST | appReport | Near expiry |
| `/appReport/Reportstorage_api/getCloseExpiryMediactionReport` | POST | appReport | Export expiry |
| `/appReport/incomeAndExpensesReport` | GET | appReport | Income/expenses |
| `/appReport/incomeAndExpensesReportExport` | GET | appReport | Export income/exp |
| `/appReport/incomeAndExpensesReportActionList` | GET | appReport | Action types |
| `/appReport/appReport_api/dailyAverageAmountReport` | GET | appReport | Daily average |
| `/appReport/appReport_api/printDailyAverageAmountReport` | GET | appReport | Export daily avg |
| `/appReport/report_api/getMedicationStatementReport` | GET | appReport | Drug statement |
| `/appReport/report_api/printMedicationStatementReport` | POST | appReport | Export statement |
| `/appReport/appReport_api/bottleNoReturnReport` | GET | appReport | Bottle no return |
| `/appReport/appReport_api/printBottleNoReturnReport` | GET | appReport | Export bottle |
| `/appReport/appReport_api/get_prescription_difference_report` | GET | appReport | Prescription diff |
| `/appReport/appReport_api/print_prescription_difference_report` | POST | appReport | Export prescript |
| `/appReport/appReport_api/get_inventory_difference_report` | GET | appReport | Inventory diff |
| `/appReport/appReport_api/print_inventory_difference_report` | POST | appReport | Export inventory |
| `/appReport/report_api/getSystemDifferenceReport` | GET | appReport | System diff |
| `/appReport/report_api/printSystemDifferenceReport` | GET | appReport | Export system |
| `/appReport/appReport_api/destroyMedicationReport` | GET | appReport | Destroy report |
| `/appReport/appReport_api/printdestroyMedicationReport` | GET | appReport | Export destroy |
| `/appReport/Reportstorage_api/remote_call_standby_query` | GET | appReport | Standby query |
| `/appReport/Reportstorage_api/remote_call_print_standby_detail_query` | GET | appReport | Export standby |
| `/appReport/Reportstorage_api/remote_call_standby_detail_query` | GET | appReport | Standby detail |
| `/appReport/appReport_api/get_system_operation_report` | GET | appReport | System operation |
| `/appReport/appReport_api/print_system_operation_report` | GET | appReport | Export operation |
| `/appReport/appReport_api/exceptionReport` | POST | appReport | Exception report |
| `/appReport/appReport_api/maintenanceReport` | POST | appReport | Maintenance report |
| `/appReport/appReport_api/printExceptionReport` | POST | appReport | Export exception |
| `/appReport/appReport_api/printMaintenanceReport` | POST | appReport | Export maintenance |
| `/appReport/appReport_api/exceptionHistoryAlertReport` | GET | appReport | Exception history |
| `/appReport/appReport_api/printExceptionHistoryAlertReport` | POST | appReport | Export history |
| `/appReport/appReport_api/get_overall_report` | GET | appReport | Overall report |
| `/appReport/appReport_api/print_overall_report` | GET | appReport | Export overall |
| `/appReport/appReport_api/get_medicine_depot_report` | GET | appReport | Depot report |
| `/appReport/appReport_api/print_medicine_depot_report` | GET | appReport | Export depot |
| `/appReport/appReport_api/get_confine_cabinet_report` | GET | appReport | Confined report |
| `/appReport/appReport_api/print_confine_cabinet_report` | GET | appReport | Export confined |
| `/appReport/cabinetInventoryReport` | GET | appReport | Cabinet inventory |
| `/appReport/cabinetInventoryReportExport` | GET | appReport | Export cabinet |
| `/appReport/appReport_api/get_cabinet_drawer_report` | GET | appReport | Drawer report |
| `/appReport/appReport_api/print_cabinet_drawer_report` | GET | appReport | Export drawer |
| `/appReport/dateBalanceReport` | GET | appReport | Date balance |
| `/appReport/dateBalanceReportExport` | GET | appReport | Export balance |
| `/appReport/blindInventoryType` | GET | appReport | Blind inventory types |
| `/appReport/appReport_api/getCabinetNameList` | GET | appReport | Cabinet name list |
| `/appReport/Reportstorage_api/getDrawerlist` | POST | appReport | Drawer list |
| `/appReport/appReport_api/get_dropList` | POST | appReport | Dropdown list |
| `/appReport/appReport_api/list_prescription_approval` | GET | appReport | Prescription list |
| `/appReport/appReport_api/get_prescription_approval` | GET | appReport | Prescription get |
| `/appReport/appReport_api/update_prescription_approval_status` | POST | appReport | Update status |

### 1.14 Utility & Other

| Endpoint | Method | Service | Description |
|----------|--------|---------|-------------|
| `/print` | POST | baseURL_print | Print service |
| `/DemoDataRestore` | POST | app001 | Demo data restore |
| `/partialRecallResetList` | GET | app001 | Partial recall reset |

---

## 2. URL Mapping Strategy

### 2.1 Base URL Consolidation

Current frontend uses 3 base URLs:
- `baseURL_P` → `/app001/` routes
- `baseURL_S` → `/app002/` routes
- `baseURL_L` → `/app003/` routes

In the unified .NET application, preserve the path prefixes:

```
/app001/*  →  AuthController, PatientController, etc. under /app001 route
/app002/*  →  InventoryController, SettingsController under /app002 route
/app003/*  →  ReturnController, RecallController, SupplyController under /app003 route
/appReport/* → ReportController under /appReport route
```

### 2.2 Controller Organization

```
Controllers/
├── App001/
│   ├── AuthController.cs         # /app001/login, getSetting, etc.
│   ├── PatientController.cs      # /app001/getPatientList, etc.
│   ├── TakeController.cs         # /app001/createPrescriptionGetTask, etc.
│   └── SettingController.cs      # /app001/getSetting
├── App002/
│   ├── InventoryController.cs    # /app002/inventory_api/*, verification_api/*
│   ├── SettingsController.cs     # /app002/setting_api/*
│   ├── CabinetController.cs      # /app002/inventory_api/get_cabinet
│   ├── CommonAmountController.cs # /app002/common_amount_api/*
│   └── BorrowController.cs       # /app002/borrow_medication_api/*
├── App003/
│   ├── ReturnController.cs       # /app003/Return_api/*, Refill_api/*
│   ├── RecallController.cs       # /app003/Recall/*
│   ├── SupplyController.cs       # /app003/Supply_api/*
│   └── StandingController.cs     # /app003/callout_api/*
├── AppReport/
│   ├── ReportController.cs       # /appReport/* base reports
│   ├── InventoryReportController.cs  # /appReport/Reportstorage_api/*
│   ├── UserController.cs         # /appReport/userList, etc.
│   └── StandingReportController.cs   # /appReport/common_amount/*
└── System/
    └── PrintController.cs        # /print
```

---

## 3. Hardware Control Module Design

### 3.1 Module Architecture

Instead of separate imadc API, hardware control becomes an internal module:

```
Services/
├── Hardware/
│   ├── IHardwareControlService.cs     # Interface
│   ├── HardwareControlService.cs      # Implementation
│   ├── SerialPortService.cs           # Serial communication
│   └── Models/
│       ├── DrawerCommand.cs
│       ├── BoxCommand.cs
│       ├── LockStatus.cs
│       └── DrawerStatus.cs
├── Communication/
│   ├── ISerialCommunicator.cs
│   └── SerialCommunicator.cs
└── State/
    └── HardwareStateManager.cs        # Track drawer/box states
```

### 3.2 Hardware API Interface

```csharp
public interface IHardwareControlService
{
    // Drawer operations
    Task<DrawerStatus> GetDrawerStatusAsync(int drawerId);
    Task<bool> OpenDrawerAsync(int drawerId);
    Task<bool> CheckDrawerAsync(int drawerId);
    
    // Box operations
    Task<BoxStatus> GetBoxStatusAsync(int drawerId, int boxId);
    Task<bool> OpenBoxAsync(int drawerId, int boxId);
    Task<bool> CheckBoxAsync(int drawerId, int boxId);
    
    // Lock operations
    Task<LockStatus> GetLockStatusAsync(int drawerId);
    Task<bool> UnlockAsync(int drawerId);
    
    // Status operations
    Task<bool> GetStatusRequestAsync(int drawerId);
    Task<bool> ReopenDrawerAsync(int drawerId);
}
```

### 3.3 Integration Points

Controllers call hardware service directly:

```csharp
[ApiController]
[Route("app002/inventory_api")]
public class InventoryController : ControllerBase
{
    private readonly IHardwareControlService _hardware;
    
    public InventoryController(IHardwareControlService hardware)
    {
        _hardware = hardware;
    }
    
    [HttpGet("auto_openBox")]
    public async Task<IActionResult> AutoOpenBox([FromQuery] int drawerId, [FromQuery] int boxId)
    {
        var result = await _hardware.OpenBoxAsync(drawerId, boxId);
        return Ok(new { success = result });
    }
}
```

---

## 4. Database Migration (PostgreSQL → MariaDB)

### 4.1 Entity Framework Setup

```csharp
// DbContext configuration for MariaDB
services.AddDbContext<AdcDbContext>(options =>
    options.UseMySql(
        configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
    ));
```

### 4.2 Key Schema Considerations

| PostgreSQL Type | MariaDB Equivalent |
|-----------------|-------------------|
| `UUID` | `CHAR(36)` or `BINARY(16)` |
| `JSONB` | `JSON` |
| `TIMESTAMP WITH TIME ZONE` | `DATETIME(3)` or `TIMESTAMP` |
| `SERIAL` | `AUTO_INCREMENT` |
| `ARRAY` | Separate table or JSON |

### 4.3 Complex Query Optimization Areas

Based on frontend usage, focus on optimizing:

1. **Inventory Queries** (`get_findMediaction`, `get_all_cabinet_stock`)
   - Heavy joins between medication, storage, drawer, box tables
   - Consider materialized views or caching

2. **Report Queries**
   - Date range based aggregations
   - Consider read replicas or separate reporting DB

3. **Action History Queries**
   - Time-series data with complex filters
   - Consider partitioning by date

---

## 5. Implementation Phases

### Phase 1: Core Infrastructure (Week 1-2)
- [ ] Project setup based on ADC-STD-api template
- [ ] MariaDB connection and EF Core setup
- [ ] Base controller classes
- [ ] Hardware control module skeleton
- [ ] Authentication & authorization middleware
- [ ] JWT token handling

### Phase 2: Basic CRUD & Settings (Week 3-4)
- [ ] User management APIs (`/app001/login`, `/app001/getUser*`, etc.)
- [ ] Settings APIs (`/app001/getSetting`)
- [ ] Cabinet/Drawer/Box management (`/app002/setting_api/*`)
- [ ] Medication management (`/app002/setting_api/get_Medication`, etc.)
- [ ] Hardware integration for basic drawer operations

### Phase 3: Core Operations (Week 5-6)
- [ ] Patient/Get operations (`/app001/*` for taking medicine)
- [ ] Return operations (`/app003/Return_api/*`)
- [ ] Inventory/Count operations (`/app002/inventory_api/*`)
- [ ] Hardware integration for box opening

### Phase 4: Advanced Operations (Week 7-8)
- [ ] Recall/Recycle operations (`/app003/Recall/*`)
- [ ] Supply/Replenish operations (`/app003/Supply_api/*`)
- [ ] Standing/Common Amount operations (`/app003/callout_api/*`, `/app002/common_amount_api/*`)
- [ ] Borrow operations (`/app002/borrow_medication_api/*`)

### Phase 5: Reports & Queries (Week 9-10)
- [ ] All report endpoints (`/appReport/*`)
- [ ] Excel export functionality
- [ ] Complex query optimization

### Phase 6: Testing & Deployment (Week 11-12)
- [ ] Integration testing with all 3 frontends
- [ ] Hardware testing with actual cabinets
- [ ] Performance testing
- [ ] Deployment setup (Docker)

---

## 6. Project Structure

```
ADC-STD.Api/
├── src/
│   ├── ADC-STD.Api/                    # Web API host
│   │   ├── Controllers/
│   │   │   ├── App001/
│   │   │   ├── App002/
│   │   │   ├── App003/
│   │   │   ├── AppReport/
│   │   │   └── System/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── Dockerfile
│   ├── ADC-STD.Core/                   # Shared core
│   │   ├── Models/
│   │   ├── Interfaces/
│   │   ├── Exceptions/
│   │   └── Extensions/
│   ├── ADC-STD.Domain/                 # Business logic
│   │   ├── Entities/
│   │   ├── Services/
│   │   └── Hardware/
│   ├── ADC-STD.Infrastructure/         # Data access
│   │   ├── Data/
│   │   │   ├── AdcDbContext.cs
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   └── Hardware/
│   └── ADC-STD.Tests/                  # Unit tests
├── tests/
│   └── IntegrationTests/
└── ADC-STD.sln
```

---

## 7. Key Technical Decisions

### 7.1 Preserving URL Compatibility
- All existing URL paths must work without changes
- Use attribute routing to match Python URL patterns exactly
- Case-insensitive routing where necessary

### 7.2 Hardware Integration
- Serial port communication via `System.IO.Ports`
- Async operations with timeout handling
- State caching to reduce serial communication
- Circuit breaker pattern for hardware failures

### 7.3 Database Migration Strategy
- EF Core with Pomelo MySQL provider
- Code-first migrations
- Separate connection strings for dev/test/prod
- Query optimization for report endpoints

### 7.4 Authentication
- JWT tokens compatible with existing frontend
- Same token format and claims
- Support for both cookie and header-based auth

---

## 8. File References

Generated from analysis:
- `client-web-api-spec.md` - imadc-client-web-2.3 API spec
- `setup-api-spec.md` - imadc-setup-v2-3 API spec
- `center-api-spec.md` - imadc-center-v2 API spec
- `dotnet-template-patterns.md` - .NET architectural patterns

---

## 9. GitHub Repository Setup

Create new repository: `FutureApp-Work/ADC-STD`

Initial structure:
```
.gitignore
README.md
src/
docs/
  ├── API_SPECIFICATION.md (this document)
  └── MIGRATION_GUIDE.md
docker-compose.yml
```

---

## 10. Next Steps

1. Review this specification with Ben
2. Create GitHub repository
3. Set up initial project structure using ADC-STD-api as template
4. Begin Phase 1 implementation
5. Parallel: Hardware module POC with actual cabinet hardware
