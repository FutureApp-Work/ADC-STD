# iMADC Client Web 2.3 - API Specification

## Overview

This document contains a comprehensive specification of all API endpoints used by the iMADC Client Web frontend application.

**Version:** 2.3.178  
**Base Configuration File:** `public/static/config_client.js`

---

## Base URLs

The application uses three different base URLs for different service categories:

| Base URL Variable | QA Environment | Test Environment | Production (Cheng Hsin) |
|-------------------|----------------|------------------|------------------------|
| `baseURL_P` | `/app001/` | `http://192.168.10.98/app001/` | `http://127.0.0.1/app001/` |
| `baseURL_S` | `/app002/` | `http://192.168.10.98/app002/` | `http://127.0.0.1/app002/` |
| `baseURL_L` | `/app003/` | `http://192.168.10.98/app003/` | `http://127.0.0.1/app003/` |

### Service Categories

- **baseURL_P**: Patient-related, Login, Permission, Settings, Take (取藥)
- **baseURL_S**: Inventory/Count (盤點), Cabinet, Unit, Common Amount, Borrow
- **baseURL_L**: Return (退藥), Recycle/Recall (回收), Replenish/Supply (撥補), Standing (常備量)

---

## Authentication

All API requests (except login) include a Bearer token in the Authorization header:

```javascript
config.headers.Authorization = `Bearer ${sessionStorage.getItem("token")}`
```

The token is obtained from the login response and stored in `sessionStorage`.

---

## API Endpoints Summary Table

| Endpoint | Method | Service | Frontend Usage Location |
|----------|--------|---------|------------------------|
| `/login` | POST | baseURL_P | `src/api/loginApi.js` |
| `/loginUseAccountOnly` | POST | baseURL_P | `src/api/loginApi.js` |
| `/doubleVerification` | POST | baseURL_P | `src/api/loginApi.js` |
| `/getSetting` | GET | baseURL_P | `src/api/setting.js` |
| `/getPatientList` | GET | baseURL_P | `src/api/patientApi.js` |
| `/getPrescriptionDetail` | GET | baseURL_P | `src/api/patientApi.js` |
| `/getStationList` | GET | baseURL_P | `src/api/patientApi.js` |
| `/getPermissionList` | GET | baseURL_P | `src/api/permissionApi.js` |
| `/inventory_api/get_cabinet` | GET | baseURL_S | `src/api/cabinetApi.js` |
| `/setting_api/get_dropList` | POST | baseURL_S | `src/api/unitApi.js` |
| `/createPrescriptionGetTask` | POST | baseURL_P | `src/api/takeApi.js` |
| `/currentGetStorage` | GET | baseURL_P | `src/api/takeApi.js` |
| `/openBox` | GET | baseURL_P | `src/api/takeApi.js` |
| `/getAction` | POST | baseURL_P | `src/api/takeApi.js` |
| `/forceInventory` | POST | baseURL_P | `src/api/takeApi.js` |
| `/boxList` | GET | baseURL_P | `src/api/takeApi.js` |
| `/checkBox` | GET | baseURL_P | `src/api/takeApi.js` |
| `/createMedicationGetTask` | POST | baseURL_P | `src/api/takeApi.js` |
| `/boxInfo` | GET | baseURL_P | `src/api/takeApi.js` |
| `/cumulativeGetMedicationKnowledgeBoxList` | GET | baseURL_P | `src/api/takeApi.js` |
| `/cumulativeGetPrescriptionDetailList` | GET | baseURL_P | `src/api/takeApi.js` |
| `/createCumulativeGetTask` | POST | baseURL_P | `src/api/takeApi.js` |
| `/cumulativeGetCurrentStorage` | GET | baseURL_P | `src/api/takeApi.js` |
| `/getBindPrescriptionDetail` | GET | baseURL_P | `src/api/takeApi.js` |
| `/getBindGetActionHistoryList` | GET | baseURL_P | `src/api/takeApi.js` |
| `/getBindGetActionHistory` | GET | baseURL_P | `src/api/takeApi.js` |
| `/getBindPrescriptionDetailCompareGetActionHistory` | GET | baseURL_P | `src/api/takeApi.js` |
| `/bindPrescriptionAndMedicationGet` | POST | baseURL_P | `src/api/takeApi.js` |
| `/getBlindInventory` | GET | baseURL_P | `src/api/takeApi.js` |
| `/updateDailyAverageAmountDescriptionByTask` | POST | baseURL_P | `src/api/takeApi.js` |
| `/getBindPrescriptionList` | GET | baseURL_P | `src/api/takeApi.js` |
| `/Return_api/creatReturnMediaction` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/reconsider_open_box` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/returnMedicationNextBox` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/returnMediactionfinish` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/findReturnMedication` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/returnMedication_check_lock` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/getReturnPage` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/creatNoPrescriptionReturntask` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/noPrescriptionReturnFinish` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/findPrescriptionMedication` | GET | baseURL_L | `src/api/returnApi.js` |
| `/Return_api/noPrescriptionReturnAbolish` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Refill_api/create_Refill_task` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Refill_api/create_Refill_task_storage` | POST | baseURL_L | `src/api/returnApi.js` |
| `/Refill_api/get_Task_detail` | GET | baseURL_L | `src/api/returnApi.js` |
| `/Refill_api/Refill_box_check` | GET | baseURL_L | `src/api/returnApi.js` |
| `/inventory_api/get_drug_info` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/Create_DrawerBox_Task` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/auto_openBox` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/reopenDrawer` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/getStatusRequest` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/checkDrawerStatus` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/inventory` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/abnormal_recode` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/get_drawer` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/Create_CabinetDrawer_Task` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/get_Drawer_detail` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/select_box` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/skip_Drawer` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/goback` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/get_drug_boxInfo` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/get_drug_Type_and_Level` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/getDrugError` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/inventory_double_verification` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/update_task_status` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/get_empty_bottle_inventory_medicine_list` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/create_empty_bottle_inventory_task` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/get_empty_bottle_medicine_drawer_box_list` | GET | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/empty_bottle_inventory_open_box` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/update_empty_bottle_inventory_task_status` | POST | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/inventory_create_blind_inventory_history` | POST | baseURL_S | `src/api/countApi.js` |
| `/verification_api/check_get_medication_list` | GET | baseURL_S | `src/api/countApi.js` |
| `/verification_api/check_inventory` | POST | baseURL_S | `src/api/countApi.js` |
| `/verification_api/get_check_question_list` | GET | baseURL_S | `src/api/countApi.js` |
| `/verification_api/verify` | POST | baseURL_S | `src/api/countApi.js` |
| `/verification_api/create_inventory_check_history` | POST | baseURL_S | `src/api/countApi.js` |
| `/verification_api/check_inventory_double_verification` | POST | baseURL_S | `src/api/countApi.js` |
| `/verification_api/delete_storage_detail` | DELETE | baseURL_S | `src/api/countApi.js` |
| `/inventory_api/get_alter_flag` | GET | baseURL_S | `src/api/countApi.js` |
| `/Supply_api/get_cabinet_low_inventory` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/supply_task` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/reconsider_open_box` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/supply_close` | GET | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/supply_in_DB` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/Drawwer_task` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/get_medication_batch_expirationDate` | GET | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/Mission_Discontinue` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/supply_Drawer_finsh` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/supply_drawer_info` | GET | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/Drawer_task_openbox` | POST | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/supply_checkdrawer` | GET | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/get_supply_source` | GET | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/get_empty_bottle_supply_list` | GET | baseURL_L | `src/api/replenishApi.js` |
| `/Supply_api/supply_medication_find_box` | GET | baseURL_L | `src/api/replenishApi.js` |
| `/Recall/recall_task` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/get_recall_box_list` | GET | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_box_info` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_open_box` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_in_DB` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_Mediaction` | GET | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/get_recall_box_info` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_box_task_done` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_box_stock_delete` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/doubleVerification` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/get_recall_box_info_list` | GET | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/clean_recall_task` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_Mediaction_no_prescription` | GET | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/create_blindinventory_history` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/Recall_box_check` | GET | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/get_unbound_recall` | GET | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/get_uabound_get_list` | GET | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/Recall_connect_recall_and_get` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/Recall/recall_box_stock_delete_have_next` | POST | baseURL_L | `src/api/recycleApi.js` |
| `/callout_api/common_amount_callout_task` | POST | baseURL_L | `src/api/standingApi.js` |
| `/callout_api/common_amount_callout_task_cancel` | POST | baseURL_L | `src/api/standingApi.js` |
| `/callout_api/common_task_box_list` | POST | baseURL_L | `src/api/standingApi.js` |
| `/callout_api/common_amount_callout_task_open_box` | POST | baseURL_L | `src/api/standingApi.js` |
| `/callout_api/common_amount_callout_box_info` | POST | baseURL_L | `src/api/standingApi.js` |
| `/callout_api/common_amount_callout_task_write` | POST | baseURL_L | `src/api/standingApi.js` |
| `/common_amount_api/get_common_amount_form` | GET | baseURL_S | `src/api/commonAmountApi.js` |
| `/common_amount_api/get_common_amount_status_list` | GET | baseURL_S | `src/api/commonAmountApi.js` |
| `/common_amount_api/get_common_amount_form_detail` | GET | baseURL_S | `src/api/commonAmountApi.js` |
| `/common_amount_api/review_common_amount_form` | POST | baseURL_S | `src/api/commonAmountApi.js` |
| `/borrow_medication_api/get_borrow_medication_form` | GET | baseURL_S | `src/api/borrowApi.js` |
| `/borrow_medication_api/get_medication` | GET | baseURL_S | `src/api/borrowApi.js` |
| `/borrow_medication_api/create_borrow_medication_form` | POST | baseURL_S | `src/api/borrowApi.js` |
| `/borrow_medication_api/get_borrow_medication_form_detail` | GET | baseURL_S | `src/api/borrowApi.js` |
| `/borrow_medication_api/get_borrow_medication_form_notify` | GET | baseURL_S | `src/api/borrowApi.js` |
| `/borrow_medication_api/create_borrow_medication_form_notify_on_read` | POST | baseURL_S | `src/api/borrowApi.js` |
| `/borrow_medication_api/delete_borrow_medication_form` | DELETE | baseURL_S | `src/api/borrowApi.js` |
| `/borrow_medication_api/update_borrow_medication_form` | POST | baseURL_S | `src/api/borrowApi.js` |
| `/print` | POST | baseURL_print | `src/api/printApi.js` |
| `/DemoDataRestore` | POST | baseURL_P | `src/api/resetDataApi.js` |
| `/partialRecallResetList` | GET | baseURL_P | `src/api/partRecallApi.js` |


---

## Detailed API Specifications by Module

### 1. Login API (`src/api/loginApi.js`)

Base URL: `baseURL_P` (window.g.baseURL_P)

#### POST `/login`
- **Description**: User login with credentials
- **Request Body**: `{ account, password, ... }`
- **Response**: `{ token, user, notCloseAlert, idleTimeout, ... }`

#### POST `/loginUseAccountOnly`
- **Description**: SSO login with account only
- **Request Body**: `{ account }`
- **Response**: `{ token, user, notCloseAlert, idleTimeout, ... }`

#### POST `/doubleVerification`
- **Description**: Double verification for sensitive operations
- **Request Body**: `{ account, password }`
- **Response**: Verification result

---

### 2. Settings API (`src/api/setting.js`)

Base URL: `baseURL_P`

#### GET `/getSetting`
- **Description**: Get application settings
- **Query Parameters**: `{ project, module, page, page_size }`
- **Response**: Settings list

---

### 3. Patient API (`src/api/patientApi.js`)

Base URL: `baseURL_P`

#### GET `/getPatientList`
- **Description**: Get list of patients
- **Query Parameters**: Query object with filter criteria
- **Response**: Patient list

#### GET `/getPrescriptionDetail`
- **Description**: Get prescription details
- **Query Parameters**: `{ prescriptionId, ... }`
- **Response**: Prescription details

#### GET `/getStationList`
- **Description**: Get nursing station list
- **Query Parameters**: Query object
- **Response**: Station list

---

### 4. Permission API (`src/api/permissionApi.js`)

Base URL: `baseURL_P`

#### GET `/getPermissionList`
- **Description**: Get user permissions
- **Query Parameters**: Query object
- **Response**: Permission list

---

### 5. Cabinet API (`src/api/cabinetApi.js`)

Base URL: `baseURL_S`

#### GET `/inventory_api/get_cabinet`
- **Description**: Get cabinet information
- **Response**: Cabinet data

---

### 6. Unit API (`src/api/unitApi.js`)

Base URL: `baseURL_S`

#### POST `/setting_api/get_dropList`
- **Description**: Get dropdown list for units/categories
- **Request Body**: `{ dataType }` (e.g., "MedicineUnit", "RegulatoryLevel", "MedicineCategory")
- **Response**: Dropdown options

---

### 7. Take API (取藥) (`src/api/takeApi.js`)

Base URL: `baseURL_P`

#### POST `/createPrescriptionGetTask`
- **Description**: Create prescription get task
- **Request Body**: Task creation data
- **Response**: Task info

#### GET `/currentGetStorage`
- **Description**: Get current storage for medication
- **Query Parameters**: `{ drawerBoxId, ... }`
- **Response**: Storage info

#### GET `/openBox`
- **Description**: Open box for taking medication
- **Query Parameters**: `{ taskId, ... }`
- **Response**: Box open result

#### POST `/getAction`
- **Description**: Record get action
- **Request Body**: Action data
- **Response**: Action result

#### POST `/forceInventory`
- **Description**: Force inventory check
- **Request Body**: Inventory data
- **Response**: Result

#### GET `/boxList`
- **Description**: Get box list
- **Query Parameters**: Filter criteria
- **Response**: Box list

#### GET `/checkBox`
- **Description**: Check box status
- **Query Parameters**: `{ boxId, ... }`
- **Response**: Box status

#### POST `/createMedicationGetTask`
- **Description**: Create medication get task (without prescription)
- **Request Body**: Task data
- **Response**: Task info

#### GET `/boxInfo`
- **Description**: Get box information
- **Query Parameters**: `{ boxId, ... }`
- **Response**: Box details

#### GET `/cumulativeGetMedicationKnowledgeBoxList`
- **Description**: Get cumulative medication box list
- **Query Parameters**: Filter criteria
- **Response**: Box list

#### GET `/cumulativeGetPrescriptionDetailList`
- **Description**: Get cumulative prescription detail list
- **Query Parameters**: Filter criteria
- **Response**: Prescription list

#### POST `/createCumulativeGetTask`
- **Description**: Create cumulative get task
- **Request Body**: Task data
- **Response**: Task info

#### GET `/cumulativeGetCurrentStorage`
- **Description**: Get cumulative current storage
- **Query Parameters**: `{ drawerBoxId, ... }`
- **Response**: Storage info

#### GET `/getBindPrescriptionDetail`
- **Description**: Get bind prescription detail
- **Query Parameters**: `{ prescriptionId, ... }`
- **Response**: Prescription detail

#### GET `/getBindGetActionHistoryList`
- **Description**: Get get action history list
- **Query Parameters**: Filter criteria
- **Response**: History list

#### GET `/getBindGetActionHistory`
- **Description**: Get single get action history
- **Query Parameters**: `{ historyId, ... }`
- **Response**: History detail

#### GET `/getBindPrescriptionDetailCompareGetActionHistory`
- **Description**: Compare prescription with get action history
- **Query Parameters**: Array of IDs
- **Response**: Comparison result

#### POST `/bindPrescriptionAndMedicationGet`
- **Description**: Bind prescription with medication get
- **Request Body**: Binding data
- **Response**: Result

#### GET `/getBlindInventory`
- **Description**: Get blind inventory data
- **Query Parameters**: Filter criteria
- **Response**: Inventory data

#### POST `/updateDailyAverageAmountDescriptionByTask`
- **Description**: Update daily average amount description
- **Request Body**: Update data
- **Response**: Result

#### GET `/getBindPrescriptionList`
- **Description**: Get bind prescription list
- **Query Parameters**: Filter criteria
- **Response**: Prescription list


---

### 8. Return API (退藥) (`src/api/returnApi.js`)

Base URL: `baseURL_L`

#### POST `/Return_api/creatReturnMediaction`
- **Description**: Create return medication task
- **Request Body**: Return data
- **Response**: Task info

#### POST `/Return_api/reconsider_open_box`
- **Description**: Reconsider open box for return
- **Request Body**: Box data
- **Response**: Result

#### POST `/Return_api/returnMedicationNextBox`
- **Description**: Get next box for return
- **Request Body**: Task data
- **Response**: Box info

#### POST `/Return_api/returnMediactionfinish`
- **Description**: Finish return medication
- **Request Body**: Finish data
- **Response**: Result

#### POST `/Return_api/findReturnMedication`
- **Description**: Find return medication
- **Request Body**: Search criteria
- **Response**: Medication list

#### POST `/Return_api/returnMedication_check_lock`
- **Description**: Check lock for return medication
- **Request Body**: Lock check data
- **Response**: Lock status

#### POST `/Return_api/getReturnPage`
- **Description**: Get return page data
- **Request Body**: Page request
- **Response**: Page data

#### POST `/Return_api/creatNoPrescriptionReturntask`
- **Description**: Create no-prescription return task
- **Request Body**: Task data
- **Response**: Task info

#### POST `/Return_api/noPrescriptionReturnFinish`
- **Description**: Finish no-prescription return
- **Request Body**: Finish data
- **Response**: Result

#### GET `/Return_api/findPrescriptionMedication`
- **Description**: Find prescription medication
- **Query Parameters**: Array of prescription IDs
- **Response**: Medication list

#### POST `/Return_api/noPrescriptionReturnAbolish`
- **Description**: Abolish no-prescription return
- **Request Body**: Abolish data
- **Response**: Result

#### POST `/Refill_api/create_Refill_task`
- **Description**: Create refill task (borrow medication)
- **Request Body**: Task data
- **Response**: Task info

#### POST `/Refill_api/create_Refill_task_storage`
- **Description**: Create refill task with storage
- **Request Body**: Task data
- **Response**: Task info

#### GET `/Refill_api/get_Task_detail`
- **Description**: Get refill task detail
- **Query Parameters**: `{ taskId, ... }`
- **Response**: Task detail

#### GET `/Refill_api/Refill_box_check`
- **Description**: Check refill box
- **Query Parameters**: `{ boxId, ... }`
- **Response**: Box status

---

### 9. Count/Inventory API (盤點) (`src/api/countApi.js`)

Base URL: `baseURL_S`

#### POST `/inventory_api/get_drug_info`
- **Description**: Get drug information
- **Request Body**: `{ cabinetId, ... }`
- **Response**: Drug info

#### POST `/inventory_api/Create_DrawerBox_Task`
- **Description**: Create drawer box task
- **Request Body**: Task data
- **Response**: Task info

#### GET `/inventory_api/auto_openBox`
- **Description**: Auto open box
- **Query Parameters**: `{ taskId, ... }`
- **Response**: Open result

#### GET `/inventory_api/reopenDrawer`
- **Description**: Reopen drawer
- **Query Parameters**: `TaskId`
- **Response**: Result

#### POST `/inventory_api/getStatusRequest`
- **Description**: Get status request
- **Request Body**: Status request data
- **Response**: Status

#### GET `/inventory_api/checkDrawerStatus`
- **Description**: Check drawer status
- **Query Parameters**: `TaskId`
- **Response**: Status

#### POST `/inventory_api/inventory`
- **Description**: Submit inventory
- **Request Body**: Inventory data
- **Response**: Result

#### POST `/inventory_api/abnormal_recode`
- **Description**: Record abnormal inventory
- **Request Body**: Abnormal data
- **Response**: Result

#### GET `/inventory_api/get_drawer`
- **Description**: Get drawer list
- **Query Parameters**: `Cabinet_id`
- **Response**: Drawer list

#### POST `/inventory_api/Create_CabinetDrawer_Task`
- **Description**: Create cabinet drawer task
- **Request Body**: Task data
- **Response**: Task info

#### POST `/inventory_api/get_Drawer_detail`
- **Description**: Get drawer detail
- **Request Body**: `{ drawerId, ... }`
- **Response**: Drawer detail

#### POST `/inventory_api/select_box`
- **Description**: Select box
- **Request Body**: `{ boxId, ... }`
- **Response**: Result

#### POST `/inventory_api/skip_Drawer`
- **Description**: Skip drawer
- **Request Body**: `{ drawerId, ... }`
- **Response**: Result

#### GET `/inventory_api/goback`
- **Description**: Go back/cancel task
- **Query Parameters**: `TaskId`
- **Response**: Result

#### GET `/inventory_api/get_drug_boxInfo`
- **Description**: Get drug box info
- **Query Parameters**: Filter criteria
- **Response**: Box info

#### GET `/inventory_api/get_drug_Type_and_Level`
- **Description**: Get drug types and levels
- **Response**: Types and levels

#### GET `/inventory_api/getDrugError`
- **Description**: Get drug error types
- **Response**: Error types

#### POST `/inventory_api/inventory_double_verification`
- **Description**: Double verification for inventory
- **Request Body**: Verification data
- **Response**: Result

#### POST `/inventory_api/update_task_status`
- **Description**: Update task status
- **Request Body**: Status data
- **Response**: Result

#### GET `/inventory_api/get_empty_bottle_inventory_medicine_list`
- **Description**: Get empty bottle inventory medicine list
- **Query Parameters**: Filter criteria
- **Response**: Medicine list

#### POST `/inventory_api/create_empty_bottle_inventory_task`
- **Description**: Create empty bottle inventory task
- **Request Body**: Task data
- **Response**: Task info

#### GET `/inventory_api/get_empty_bottle_medicine_drawer_box_list`
- **Description**: Get empty bottle medicine drawer box list
- **Query Parameters**: Filter criteria
- **Response**: Box list

#### POST `/inventory_api/empty_bottle_inventory_open_box`
- **Description**: Open box for empty bottle inventory
- **Request Body**: Box data
- **Response**: Result

#### POST `/inventory_api/update_empty_bottle_inventory_task_status`
- **Description**: Update empty bottle inventory task status
- **Request Body**: Status data
- **Response**: Result

#### POST `/inventory_api/inventory_create_blind_inventory_history`
- **Description**: Create blind inventory history
- **Request Body**: History data
- **Response**: Result

#### GET `/inventory_api/get_alter_flag`
- **Description**: Get alert flag
- **Query Parameters**: Filter criteria
- **Response**: Alert flags

---

### 10. Verification API (查核) (`src/api/countApi.js`)

Base URL: `baseURL_S`

#### GET `/verification_api/check_get_medication_list`
- **Description**: Check get medication list
- **Query Parameters**: Array of IDs
- **Response**: Medication list

#### POST `/verification_api/check_inventory`
- **Description**: Check inventory
- **Request Body**: Inventory data
- **Response**: Result

#### GET `/verification_api/get_check_question_list`
- **Description**: Get check question list
- **Response**: Question list

#### POST `/verification_api/verify`
- **Description**: Verify inventory
- **Request Body**: Verification data
- **Response**: Result

#### POST `/verification_api/create_inventory_check_history`
- **Description**: Create inventory check history
- **Request Body**: History data
- **Response**: Result

#### POST `/verification_api/check_inventory_double_verification`
- **Description**: Double verification for check inventory
- **Request Body**: Verification data
- **Response**: Result

#### DELETE `/verification_api/delete_storage_detail`
- **Description**: Delete storage detail
- **Query Parameters**: Array of IDs
- **Response**: Result


---

### 11. Replenish/Supply API (撥補) (`src/api/replenishApi.js`)

Base URL: `baseURL_L`

#### POST `/Supply_api/get_cabinet_low_inventory`
- **Description**: Get cabinet low inventory items
- **Request Body**: `{ cabinetId, ... }`
- **Response**: Low inventory list

#### POST `/Supply_api/supply_task`
- **Description**: Create supply task
- **Request Body**: Task data
- **Response**: Task info

#### POST `/Supply_api/reconsider_open_box`
- **Description**: Reconsider open box for supply
- **Request Body**: Box data
- **Response**: Result

#### GET `/Supply_api/supply_close`
- **Description**: Close supply task
- **Query Parameters**: `{ taskId, ... }`
- **Response**: Result

#### POST `/Supply_api/supply_in_DB`
- **Description**: Supply into database
- **Request Body**: Supply data
- **Response**: Result

#### POST `/Supply_api/Drawwer_task`
- **Description**: Create drawer task for supply
- **Request Body**: Task data
- **Response**: Task info

#### GET `/Supply_api/get_medication_batch_expirationDate`
- **Description**: Get medication batch expiration dates
- **Query Parameters**: `{ medicationId, ... }`
- **Response**: Expiration dates

#### POST `/Supply_api/Mission_Discontinue`
- **Description**: Discontinue supply mission
- **Request Body**: Mission data
- **Response**: Result

#### POST `/Supply_api/supply_Drawer_finsh`
- **Description**: Finish supply drawer
- **Request Body**: Drawer data
- **Response**: Result

#### GET `/Supply_api/supply_drawer_info`
- **Description**: Get supply drawer info
- **Query Parameters**: `{ drawerId, ... }`
- **Response**: Drawer info

#### POST `/Supply_api/Drawer_task_openbox`
- **Description**: Open box for drawer task
- **Request Body**: Task data
- **Response**: Result

#### GET `/Supply_api/supply_checkdrawer`
- **Description**: Check supply drawer
- **Query Parameters**: `{ drawerId, ... }`
- **Response**: Drawer status

#### GET `/Supply_api/get_supply_source`
- **Description**: Get supply source
- **Query Parameters**: Filter criteria
- **Response**: Source list

#### GET `/Supply_api/get_empty_bottle_supply_list`
- **Description**: Get empty bottle supply list
- **Query Parameters**: Filter criteria
- **Response**: Supply list

#### GET `/Supply_api/supply_medication_find_box`
- **Description**: Find box for medication supply
- **Query Parameters**: Array of medication IDs
- **Response**: Box info

---

### 12. Recycle/Recall API (回收) (`src/api/recycleApi.js`)

Base URL: `baseURL_L`

#### POST `/Recall/recall_task`
- **Description**: Create recall task
- **Request Body**: Task data
- **Response**: Task info

#### GET `/Recall/get_recall_box_list`
- **Description**: Get recall box list
- **Query Parameters**: Filter criteria
- **Response**: Box list

#### POST `/Recall/recall_box_info`
- **Description**: Get recall box info
- **Request Body**: `{ boxId, ... }`
- **Response**: Box info

#### POST `/Recall/recall_open_box`
- **Description**: Open box for recall
- **Request Body**: Box data
- **Response**: Result

#### POST `/Recall/recall_in_DB`
- **Description**: Recall into database
- **Request Body**: Recall data
- **Response**: Result

#### GET `/Recall/recall_Mediaction`
- **Description**: Get recall medication
- **Query Parameters**: Filter criteria
- **Response**: Medication list

#### POST `/Recall/get_recall_box_info`
- **Description**: Get recall box info (alternative endpoint)
- **Request Body**: `{ boxId, ... }`
- **Response**: Box info

#### POST `/Recall/recall_box_task_done`
- **Description**: Mark recall box task as done
- **Request Body**: Task data
- **Response**: Result

#### POST `/Recall/recall_box_stock_delete`
- **Description**: Delete recall box stock
- **Request Body**: Stock data
- **Response**: Result

#### POST `/Recall/doubleVerification`
- **Description**: Double verification for recall
- **Request Body**: Verification data
- **Response**: Result

#### GET `/Recall/get_recall_box_info_list`
- **Description**: Get recall box info list
- **Query Parameters**: Filter criteria
- **Response**: Box list

#### POST `/Recall/clean_recall_task`
- **Description**: Clean recall task
- **Request Body**: Task data
- **Response**: Result

#### GET `/Recall/recall_Mediaction_no_prescription`
- **Description**: Get recall medication without prescription
- **Query Parameters**: Filter criteria
- **Response**: Medication list

#### POST `/Recall/create_blindinventory_history`
- **Description**: Create blind inventory history for recall
- **Request Body**: History data
- **Response**: Result

#### GET `/Recall/Recall_box_check`
- **Description**: Check recall box
- **Query Parameters**: `{ boxId, ... }`
- **Response**: Box status

#### GET `/Recall/get_unbound_recall`
- **Description**: Get unbound recall items
- **Query Parameters**: Filter criteria
- **Response**: Recall list

#### GET `/Recall/get_uabound_get_list`
- **Description**: Get unbound get list for recall
- **Query Parameters**: Filter criteria
- **Response**: Get list

#### POST `/Recall/Recall_connect_recall_and_get`
- **Description**: Connect recall with get action
- **Request Body**: Connection data
- **Response**: Result

#### POST `/Recall/recall_box_stock_delete_have_next`
- **Description**: Delete recall box stock and check for next
- **Request Body**: Stock data
- **Response**: Result with next flag

---

### 13. Standing/Common Amount API (常備量) (`src/api/standingApi.js`)

Base URL: `baseURL_L`

#### POST `/callout_api/common_amount_callout_task`
- **Description**: Create common amount callout task
- **Request Body**: Task data
- **Response**: Task info

#### POST `/callout_api/common_amount_callout_task_cancel`
- **Description**: Cancel common amount callout task
- **Request Body**: Task data
- **Response**: Result

#### POST `/callout_api/common_task_box_list`
- **Description**: Get common task box list
- **Request Body**: Filter data
- **Response**: Box list

#### POST `/callout_api/common_amount_callout_task_open_box`
- **Description**: Open box for common amount callout
- **Request Body**: Box data
- **Response**: Result

#### POST `/callout_api/common_amount_callout_box_info`
- **Description**: Get common amount callout box info
- **Request Body**: Box data
- **Response**: Box info

#### POST `/callout_api/common_amount_callout_task_write`
- **Description**: Write common amount callout task
- **Request Body**: Task data
- **Response**: Result

---

### 14. Common Amount Status API (`src/api/commonAmountApi.js`)

Base URL: `baseURL_S`

#### GET `/common_amount_api/get_common_amount_form`
- **Description**: Get common amount form
- **Query Parameters**: Filter criteria
- **Response**: Form data

#### GET `/common_amount_api/get_common_amount_status_list`
- **Description**: Get common amount status list
- **Query Parameters**: Filter criteria
- **Response**: Status list

#### GET `/common_amount_api/get_common_amount_form_detail`
- **Description**: Get common amount form detail
- **Query Parameters**: `{ common_amount_form_id }`
- **Response**: Form detail

#### POST `/common_amount_api/review_common_amount_form`
- **Description**: Review common amount form
- **Request Body**: Review data
- **Response**: Result


---

### 15. Borrow Medication API (借藥) (`src/api/borrowApi.js`)

Base URL: `baseURL_S`

#### GET `/borrow_medication_api/get_borrow_medication_form`
- **Description**: Get borrow medication form list
- **Query Parameters**: Array of form IDs
- **Response**: Form list

#### GET `/borrow_medication_api/get_medication`
- **Description**: Get medication for borrowing
- **Query Parameters**: Filter criteria
- **Response**: Medication list

#### POST `/borrow_medication_api/create_borrow_medication_form`
- **Description**: Create borrow medication form
- **Request Body**: Form data
- **Response**: Created form

#### GET `/borrow_medication_api/get_borrow_medication_form_detail`
- **Description**: Get borrow medication form detail
- **Query Parameters**: Array of form IDs
- **Response**: Form detail

#### GET `/borrow_medication_api/get_borrow_medication_form_notify`
- **Description**: Get borrow medication form notifications
- **Response**: Notification list

#### POST `/borrow_medication_api/create_borrow_medication_form_notify_on_read`
- **Description**: Mark notification as read
- **Request Body**: `{ borrow_medication_form_id }`
- **Response**: Result

#### DELETE `/borrow_medication_api/delete_borrow_medication_form`
- **Description**: Delete borrow medication form
- **Query Parameters**: `{ borrow_medication_form_id }`
- **Response**: Result

#### POST `/borrow_medication_api/update_borrow_medication_form`
- **Description**: Update borrow medication form
- **Request Body**: Form data
- **Response**: Updated form

---

### 16. Print API (`src/api/printApi.js`)

Base URL: `getSettingValue('baseURL_print')` (configurable)

#### POST `/print`
- **Description**: Print document
- **Request Body**: Print data
- **Response**: Print result

---

### 17. Reset Data API (`src/api/resetDataApi.js`)

Base URL: `baseURL_P`

#### POST `/DemoDataRestore`
- **Description**: Restore demo data
- **Request Body**: Restore data
- **Response**: Result

---

### 18. Partial Recall API (`src/api/partRecallApi.js`)

Base URL: `baseURL_P`

#### GET `/partialRecallResetList`
- **Description**: Get partial recall reset list
- **Query Parameters**: Filter criteria
- **Response**: Reset list

---

## Configurable External URLs

The following API endpoints are configured via settings and called directly using axios:

### From `src/App.vue`

#### Language Files (GET)
- **URLs**: 
  - `{baseUrl}/static/lang/tw.json`
  - `{baseUrl}/static/lang/en.json`
  - `{baseUrl}/static/lang/jp.json`
- **Description**: Load i18n language files
- **Response**: Translation key-value pairs

### From `src/components/header.vue`

#### GET `getSettingValue("closeWindowUrl")`
- **Description**: Close window URL for SSO logout
- **Query Parameters**: `{ uri: self_URL }`
- **Called on**: Logout when SSO is enabled

### From `src/components/menu/take-operate-content.vue`

#### POST `getSettingValue("SettingScanTakeUrl")`
- **Description**: Scan QR code for taking medication
- **Request Body**: `{ qrcode }`
- **Response**: `{ Result, Message, PatId, PatName, PatNo, CumulativeGetThreshold }`

### From `src/components/menu/return-operate-content.vue`

#### POST `getSettingValue("ScanReturnUrl")`
- **Description**: Scan QR code for return
- **Request Body**: `{ qrcode }`
- **Response**: `{ Result, Message }`

### From `src/components/menu/recycle-operate-content.vue`

#### POST `getSettingValue("ScanRecycleUrl")`
- **Description**: Scan QR code for recycle
- **Request Body**: `{ qrcode }`
- **Response**: `{ Result, Message }`

### From `src/views/Return/return-delete-prescription.vue`

#### GET `getSettingValue("DeletePrescriptionUrl")`
- **Description**: Delete prescription
- **Query Parameters**: `{ PatNo, ReturnSheet }`
- **Response**: `{ Result, Message, Data }`

---

## WebSocket Connections

**No WebSocket connections were found** in the codebase.

The application uses HTTP polling via axios for all real-time communication needs.

---

## Request/Response Data Structures

### Common Headers

All authenticated requests include:
```
Authorization: Bearer {token}
Content-Type: application/json
```

### Common Response Format

Most API responses follow this structure:
```json
{
  "success": boolean,
  "sysCode": string,
  "message": string,
  "data": object | array
}
```

### Token Storage

- **Storage**: `sessionStorage`
- **Key**: `token`
- **User Data**: Stored in `sessionStorage` with key `user`

### Settings Storage

Settings are fetched on app initialization and stored in:
- **Storage**: `sessionStorage`
- **Key**: `settingData`
- **Format**: JSON array of `{ key, value }` objects

---

## API Module Organization

| Module | File | Base URL | Description |
|--------|------|----------|-------------|
| Login | `loginApi.js` | baseURL_P | Authentication |
| Settings | `setting.js` | baseURL_P | App configuration |
| Patient | `patientApi.js` | baseURL_P | Patient data |
| Permission | `permissionApi.js` | baseURL_P | User permissions |
| Cabinet | `cabinetApi.js` | baseURL_S | Cabinet management |
| Unit | `unitApi.js` | baseURL_S | Dropdown data |
| Count | `countApi.js` | baseURL_S | Inventory operations |
| Take | `takeApi.js` | baseURL_P | Medication dispensing |
| Return | `returnApi.js` | baseURL_L | Medication returns |
| Replenish | `replenishApi.js` | baseURL_L | Supply operations |
| Recycle | `recycleApi.js` | baseURL_L | Recall operations |
| Standing | `standingApi.js` | baseURL_L | Common amount operations |
| Common Amount | `commonAmountApi.js` | baseURL_S | Common amount status |
| Borrow | `borrowApi.js` | baseURL_S | Borrow medication |
| Print | `printApi.js` | Configurable | Printing service |
| Reset Data | `resetDataApi.js` | baseURL_P | Demo data reset |
| Partial Recall | `partRecallApi.js` | baseURL_P | Partial recall list |

---

## Notes

1. **Token-based Authentication**: All APIs except login require a Bearer token in the Authorization header.

2. **Multiple Base URLs**: The application uses 3 different base URLs to separate concerns:
   - `baseURL_P`: Primary patient/medication operations
   - `baseURL_S`: System/inventory operations  
   - `baseURL_L`: Logistics operations (return, supply, recall)

3. **Configurable URLs**: Several endpoints (print, scan, close window) are configurable via settings fetched from the backend.

4. **No WebSockets**: The application relies entirely on HTTP requests; no WebSocket connections are used.

5. **Error Handling**: Errors use a standardized `errorSysCode` system for consistent error display.

6. **SSO Support**: The application supports Single Sign-On via configurable SSO URLs.

