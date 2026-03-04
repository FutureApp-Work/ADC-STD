# iMADC Center V2 API Specification

## Overview

This document contains all API endpoints used by the imadc-center-v2 frontend application.

**Base URL Configuration:**
- Base URL is configured in `/public/static/config_report.js` as `window.g.baseURL`
- Default: `"/"` (relative to current host)
- Axios default baseURL is set from `window.g.baseURL` in `/src/api/api.js`

**Authentication:**
- JWT Bearer token authentication
- Token is stored in `sessionStorage` key: `nowToken`
- Token is passed in Authorization header: `Authorization: 'Bearer ' + token`
- Token expiration handling in `/src/views/error-token.js`

---

## Summary Table

| Endpoint | Method | Frontend Usage Location | Description |
|----------|--------|------------------------|-------------|
| `/app001/getSetting` | GET | `get-config.js` | Get system settings/config |
| `/appReport/login` | POST | `login.vue` | User login |
| `/appReport/changePassword` | POST | `login.vue`, `main-theme.vue` | Change password |
| `/appReport/loginUseAccountOnly` | POST | `App.vue` | SSO login with account only |
| `/appReport/appReport_api/getCabinetNameList` | GET | Multiple views | Get cabinet name list |
| `/appReport/Reportstorage_api/getDrawerlist` | POST | Multiple views | Get drawer list by cabinet |
| `/appReport/Reportstorage_api/get_findMediaction` | POST | `stock-list.vue` | Real-time inventory |
| `/appReport/Reportstorage_api/get_findMediaction_excel` | GET | `stock-list.vue` | Export inventory to Excel |
| `/appReport/Reportstorage_api/get_all_cabinet_stock` | POST | `inventory-stock-medication.vue` | Get all cabinet stock |
| `/appReport/Reportstorage_api/export_all_cabinet_stock` | POST | `inventory-stock-medication.vue` | Export all cabinet stock |
| `/appReport/Reportstorage_api/get_low_medication_list` | POST | `management-low-safe.vue`, `stock-lowSafeCount.vue` | Get low medication list |
| `/appReport/Reportstorage_api/getLowMedicationListReport` | POST | `management-low-safe.vue`, `stock-lowSafeCount.vue` | Export low medication report |
| `/appReport/Reportstorage_api/get_findCloseExpiryMediaction` | POST | `management-near-expiry-date.vue`, `stock-closeExpiry.vue` | Get near-expiry medications |
| `/appReport/Reportstorage_api/getCloseExpiryMediactionReport` | POST | `management-near-expiry-date.vue`, `stock-closeExpiry.vue` | Export near-expiry report |
| `/appReport/incomeAndExpensesReport` | GET | `drug-inventory.vue`, `record-list.vue` | Income/expenses report |
| `/appReport/incomeAndExpensesReportExport` | GET | `drug-inventory.vue`, `record-list.vue` | Export income/expenses report |
| `/appReport/incomeAndExpensesReportActionList` | GET | `main-theme.vue` | Get action types for report |
| `/appReport/getMedicationKnowledgeCodeList` | GET | Multiple views | Get medication knowledge codes |
| `/appReport/appReport_api/dailyAverageAmountReport` | GET | `query-averageDailyDose.vue` | Daily average amount report |
| `/appReport/appReport_api/printDailyAverageAmountReport` | GET | `query-averageDailyDose.vue` | Export daily average report |
| `/appReport/report_api/getMedicationStatementReport` | GET | `report-controlled-drug.vue` | Controlled drug statement |
| `/appReport/report_api/printMedicationStatementReport` | POST | `report-controlled-drug.vue` | Export controlled drug statement |
| `/appReport/appReport_api/bottleNoReturnReport` | GET | `management-bottle.vue`, `query-bottleNoReturn.vue` | Bottle not returned report |
| `/appReport/appReport_api/printBottleNoReturnReport` | GET | `management-bottle.vue`, `query-bottleNoReturn.vue` | Export bottle report |
| `/appReport/appReport_api/get_prescription_difference_report` | GET | `report-prescription-diff.vue`, `query-prescriptionDiff.vue` | Prescription difference report |
| `/appReport/appReport_api/print_prescription_difference_report` | POST | `report-prescription-diff.vue`, `query-prescriptionDiff.vue` | Export prescription diff |
| `/appReport/appReport_api/get_inventory_difference_report` | GET | `query-inventoryDiff.vue` | Inventory difference report |
| `/appReport/appReport_api/print_inventory_difference_report` | POST | `query-inventoryDiff.vue` | Export inventory diff |
| `/appReport/report_api/getSystemDifferenceReport` | GET | `report-system.vue` | System difference report |
| `/appReport/report_api/printSystemDifferenceReport` | GET | `report-system.vue` | Export system diff |
| `/appReport/appReport_api/destroyMedicationReport` | GET | `query-destroyMed.vue`, `print-destroy-medication.vue` | Destroy medication report |
| `/appReport/appReport_api/printdestroyMedicationReport` | GET | `query-destroyMed.vue`, `print-destroy-medication.vue` | Export destroy report |
| `/appReport/Reportstorage_api/remote_call_standby_query` | GET | `query-remoteCallStandby.vue`, `print-standby-query.vue` | Remote call standby query |
| `/appReport/Reportstorage_api/remote_call_print_standby_detail_query` | GET | `query-remoteCallStandby.vue`, `print-standby-query.vue` | Export standby query |
| `/appReport/Reportstorage_api/remote_call_standby_detail_query` | GET | `query-remoteCallStandbyDetail.vue`, `print-standby-query.vue` | Standby detail query |
| `/appReport/appReport_api/get_system_operation_report` | GET | `query-systemOper.vue`, `report-system.vue` | System operation report |
| `/appReport/appReport_api/print_system_operation_report` | GET | `query-systemOper.vue` | Export system operation |
| `/appReport/common_amount/create_common_amount_form` | POST | `call-outInitiate.vue` | Create common amount form |
| `/appReport/common_amount/get_medication` | GET | `call-outInitiate.vue` | Get medication (fuzzy search) |
| `/appReport/common_amount/get_common_amount_form_list` | GET | `standing-table.vue` | Get common amount form list |
| `/appReport/appReport_api/exceptionReport` | POST | `abnormal-list.vue` | Exception report |
| `/appReport/appReport_api/maintenanceReport` | POST | `maintanin-list.vue` | Maintenance report |
| `/appReport/appReport_api/printExceptionReport` | POST | `abnormal-list.vue` | Export exception report |
| `/appReport/appReport_api/printMaintenanceReport` | POST | `maintanin-list.vue` | Export maintenance report |
| `/appReport/appReport_api/exceptionHistoryAlertReport` | GET | `abnormal-warning.vue`, `abnormal-list.vue` | Exception history alert |
| `/appReport/appReport_api/printExceptionHistoryAlertReport` | POST | `abnormal-list.vue` | Export exception history |
| `/appReport/borrow_medication/get_borrow_medication_form_list` | GET | `loan-medicines.vue` | Get borrow medication list |
| `/appReport/borrow_medication/review_borrow_medication_form` | POST | `loan-medicines.vue` | Review borrow medication |
| `/appReport/borrow_medication/get_borrow_medication_form_history` | GET | `application-details.vue` | Get borrow history |
| `/appReport/borrow_medication/return_medication` | POST | `loan-medicines.vue` | Return medication |
| `/appReport/userList` | GET | `account-list.vue` | Get user list |
| `/appReport/getImage/{id}` | GET | `account-list.vue` | Get user image |
| `/appReport/createUser` | POST | `account-settings.vue` | Create user |
| `/appReport/updateUser` | POST | `account-settings.vue` | Update user |
| `/appReport/deleteUser/{id}` | DELETE | `account-list.vue` | Delete user |
| `/appReport/getUser` | GET | `account-list.vue` | Get single user |
| `/appReport/roleList` | GET | `account-list.vue` | Get role list |
| `/appReport/dateBalanceReport` | GET | `date-balance.vue`, `record-list.vue`, `report-date-balance.vue` | Date balance report |
| `/appReport/dateBalanceReportExport` | GET | `date-balance.vue`, `record-list.vue`, `report-date-balance.vue` | Export date balance |
| `/appReport/appReport_api/get_dropList` | POST | Multiple views | Get dropdown lists |
| `/appReport/blindInventoryType` | GET | `report-summary.vue`, `report-inventory.vue`, `operation-list.vue` | Get blind inventory types |
| `/appReport/appReport_api/get_overall_report` | GET | `operation-overall.vue` | Overall operation report |
| `/appReport/appReport_api/print_overall_report` | GET | `operation-overall.vue` | Export overall report |
| `/appReport/appReport_api/get_medicine_depot_report` | GET | `operation-depot.vue` | Medicine depot report |
| `/appReport/appReport_api/print_medicine_depot_report` | GET | `operation-depot.vue` | Export depot report |
| `/appReport/appReport_api/get_confine_cabinet_report` | GET | `operation-confine.vue` | Confined cabinet report |
| `/appReport/appReport_api/print_confine_cabinet_report` | GET | `operation-confine.vue` | Export confined report |
| `/appReport/cabinetInventoryReport` | GET | `operation-inventory.vue` | Cabinet inventory report |
| `/appReport/cabinetInventoryReportExport` | GET | `operation-inventory.vue` | Export cabinet inventory |
| `/appReport/appReport_api/get_cabinet_drawer_report` | GET | `operation-drawer.vue` | Cabinet drawer report |
| `/appReport/appReport_api/print_cabinet_drawer_report` | GET | `operation-drawer.vue` | Export drawer report |
| `/appReport/appReport_api/list_prescription_approval` | GET | `prescription-list.vue` | List prescription approvals |
| `/appReport/appReport_api/get_prescription_approval` | GET | `prescription-list.vue`, `prescription-detail.vue` | Get prescription approval |
| `/appReport/appReport_api/update_prescription_approval_status` | POST | `prescription-detail.vue` | Update prescription status |

---

## Authentication & Token Handling

### Token Storage
- Token is stored in `sessionStorage` with key `nowToken`
- Token is retrieved via: `sessionStorage.getItem("nowToken")`

### Token Usage
All authenticated API calls include the token in the Authorization header:
```javascript
axios.get(api.endpoint, { 
  headers: { Authorization: 'Bearer ' + nowToken } 
})
```

### Token Expiration Handling
Token errors are handled in `/src/views/error-token.js`:
- **401 Unauthorized**: Token expired - calls `ClearLogin()` and shows warning message
- **500/502 API Error**: Shows API error message

### SSO Configuration
SSO settings stored in `localStorage`:
- `sso`: 'true' or 'false' - Enable SSO login
- `sso_url`: SSO login page URL
- `sso_callback`: Callback URL after SSO login
- `closeWindowUrl`: URL to clear SSO session

---

## API Modules Summary

### 1. Authentication Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/app001/getSetting` | GET | Get system settings |
| `/appReport/login` | POST | User login with credentials |
| `/appReport/loginUseAccountOnly` | POST | SSO login (account only) |
| `/appReport/changePassword` | POST | Change user password |

### 2. Cabinet & Inventory Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/appReport/appReport_api/getCabinetNameList` | GET | List all cabinets |
| `/appReport/Reportstorage_api/getDrawerlist` | POST | Get drawers by cabinet |
| `/appReport/Reportstorage_api/get_findMediaction` | POST | Real-time inventory query |
| `/appReport/Reportstorage_api/get_all_cabinet_stock` | POST | Cross-cabinet stock |
| `/appReport/Reportstorage_api/get_low_medication_list` | POST | Low stock medications |
| `/appReport/Reportstorage_api/get_findCloseExpiryMediaction` | POST | Near-expiry medications |

### 3. Reporting Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/appReport/incomeAndExpensesReport` | GET | Income/expenses report |
| `/appReport/appReport_api/dailyAverageAmountReport` | GET | Daily average dosage report |
| `/appReport/report_api/getMedicationStatementReport` | GET | Controlled drug statement |
| `/appReport/appReport_api/bottleNoReturnReport` | GET | Bottle not returned report |
| `/appReport/appReport_api/get_prescription_difference_report` | GET | Prescription diff report |
| `/appReport/report_api/getSystemDifferenceReport` | GET | System diff report |
| `/appReport/appReport_api/destroyMedicationReport` | GET | Destroy medication report |
| `/appReport/appReport_api/get_system_operation_report` | GET | System operation report |

### 4. Common Amount (Standing Stock) Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/appReport/common_amount/create_common_amount_form` | POST | Create transfer form |
| `/appReport/common_amount/get_medication` | GET | Search medications |
| `/appReport/common_amount/get_common_amount_form_list` | GET | List transfer forms |

### 5. Borrow Medication Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/appReport/borrow_medication/get_borrow_medication_form_list` | GET | List borrow forms |
| `/appReport/borrow_medication/review_borrow_medication_form` | POST | Approve/reject borrow |
| `/appReport/borrow_medication/get_borrow_medication_form_history` | GET | Get borrow history |
| `/appReport/borrow_medication/return_medication` | POST | Return medication |

### 6. User Management Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/appReport/userList` | GET | List users |
| `/appReport/getUser` | GET | Get single user |
| `/appReport/createUser` | POST | Create new user |
| `/appReport/updateUser` | POST | Update user |
| `/appReport/deleteUser/{id}` | DELETE | Delete user |
| `/appReport/roleList` | GET | List roles |
| `/appReport/getImage/{id}` | GET | Get user photo |

### 7. Prescription Approval Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/appReport/appReport_api/list_prescription_approval` | GET | List prescriptions |
| `/appReport/appReport_api/get_prescription_approval` | GET | Get prescription details |
| `/appReport/appReport_api/update_prescription_approval_status` | POST | Update prescription status |

### 8. Exception & Maintenance Module
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/appReport/appReport_api/exceptionReport` | POST | Exception records |
| `/appReport/appReport_api/maintenanceReport` | POST | Maintenance records |
| `/appReport/appReport_api/exceptionHistoryAlertReport` | GET | Exception alerts |

---

## Detailed API Documentation

### 1. Authentication APIs

#### Login
```
POST /appReport/login
```
**Request Body:**
```json
{
  "account": "string",
  "password": "string"
}
```
**Response:**
```json
{
  "success": true,
  "data": {
    "token": "jwt_token_string",
    "idleTimeout": 300,
    "user": {
      "account": "string",
      "name": "string",
      "photo": "string",
      "cabinetRoleId": 4,
      "permissionCodeList": [...],
      "isNeedToChangePassword": false
    }
  }
}
```
**Frontend Usage:** `login.vue`

---

#### SSO Login (Account Only)
```
POST /appReport/loginUseAccountOnly
```
**Request Body:**
```json
{
  "account": "string"
}
```
**Frontend Usage:** `App.vue`

---

#### Change Password
```
POST /appReport/changePassword
```
**Request Body:**
```json
{
  "account": "string",
  "oldPassword": "string",
  "newPassword": "string"
}
```
**Frontend Usage:** `login.vue`, `main-theme.vue`

---

### 2. Cabinet APIs

#### Get Cabinet Name List
```
GET /appReport/appReport_api/getCabinetNameList
```
**Headers:**
```
Authorization: Bearer {token}
```
**Response:**
```json
{
  "success": true,
  "data": [
    {
      "cabinetId": 1,
      "cabinetName": "string"
    }
  ]
}
```
**Frontend Usage:** Most views (drug-inventory.vue, account-list
.vue, etc.)

---

#### Get Drawer List
```
POST /appReport/Reportstorage_api/getDrawerlist
```
**Headers:**
```
Authorization: Bearer {token}
```
**Request Body:**
```json
{
  "cabinetId": 1
}
```
**Frontend Usage:** `management-near-expiry-date.vue`, `management-low-safe.vue`, `stock-list.vue`

---

### 3. Inventory APIs

#### Get Real-time Inventory
```
POST /appReport/Reportstorage_api/get_findMediaction
```
**Headers:**
```
Authorization: Bearer {token}
```
**Frontend Usage:** `stock-list.vue`

---

#### Get All Cabinet Stock
```
POST /appReport/Reportstorage_api/get_all_cabinet_stock
```
**Headers:**
```
Authorization: Bearer {token}
```
**Frontend Usage:** `inventory-stock-medication.vue`

---

#### Export All Cabinet Stock
```
POST /appReport/Reportstorage_api/export_all_cabinet_stock
```
**Headers:**
```
Authorization: Bearer {token}
```
**Response:** Excel file (xlsx)
**Frontend Usage:** `inventory-stock-medication.vue`

---

#### Get Low Medication List
```
POST /appReport/Reportstorage_api/get_low_medication_list
```
**Headers:**
```
Authorization: Bearer {token}
```
**Frontend Usage:** `management-low-safe.vue`, `stock-lowSafeCount.vue`

---

#### Get Near-Expiry Medications
```
POST /appReport/Reportstorage_api/get_findCloseExpiryMediaction
```
**Headers:**
```
Authorization: Bearer {token}
```
**Frontend Usage:** `management-near-expiry-date.vue`, `stock-closeExpiry.vue`

---

### 4. Report APIs

#### Income and Expenses Report
```
GET /appReport/incomeAndExpensesReport
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinetId: number
startDate: string
endDate: string
actionTypes: array
```
**Frontend Usage:** `drug-inventory.vue`, `record-list.vue`

---

#### Daily Average Amount Report
```
GET /appReport/appReport_api/dailyAverageAmountReport
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinetId: number
startDate: string
endDate: string
controlled_drug: string
```
**Frontend Usage:** `query-averageDailyDose.vue`

---

#### Controlled Drug Statement Report
```
GET /appReport/report_api/getMedicationStatementReport
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinetId: number
startDate: string
endDate: string
controlledDrug: string
```
**Frontend Usage:** `report-controlled-drug.vue`

---

#### Prescription Difference Report
```
GET /appReport/appReport_api/get_prescription_difference_report
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinetId: number
startDate: string
endDate: string
controlled_drug: string
```
**Frontend Usage:** `report-prescription-diff.vue`, `query-prescriptionDiff.vue`

---

#### System Difference Report
```
GET /appReport/report_api/getSystemDifferenceReport
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinetId: number
startDate: string
endDate: string
controlled_drug: string
```
**Frontend Usage:** `report-system.vue`

---

#### Destroy Medication Report
```
GET /appReport/appReport_api/destroyMedicationReport
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinetId: number
startDate: string
endDate: string
controlled_drug: string
```
**Frontend Usage:** `query-destroyMed.vue`, `print-destroy-medication.vue`

---

### 5. Common Amount (Standing Stock) APIs

#### Create Common Amount Form
```
POST /appReport/common_amount/create_common_amount_form
```
**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```
**Request Body:**
```json
{
  "applicant_user_name": "string",
  "apply_type": "TransferIn|TransferOut",
  "cabinet_id": 1,
  "medication_detail": [
    {
      "delta_amount": 10,
      "medication_id": 1
    }
  ]
}
```
**Frontend Usage:** `call-outInitiate.vue`

---

#### Get Medication (Fuzzy Search)
```
GET /appReport/common_amount/get_medication
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinet_id: number
medication_name: string
page: number
pre_page: number
```
**Frontend Usage:** `call-outInitiate.vue`

---

#### Get Common Amount Form List
```
GET /appReport/common_amount/get_common_amount_form_list
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinet_id: number
page: number
pre_page: number
apply_type: string
common_amount_form_status: string
medication_name: string
start_date: string
end_date: string
sort_column: string
sort_type: asc|desc
```
**Frontend Usage:** `standing-table.vue`

---

### 6. Borrow Medication APIs

#### Get Borrow Medication Form List
```
GET /appReport/borrow_medication/get_borrow_medication_form_list
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
cabinet_id: number
page: number
pre_page: number
borrow_medication_form_status: array
start_date: string
end_date: string
sort_column: string
sort_type: asc|desc
```
**Frontend Usage:** `loan-medicines.vue`

---

#### Review Borrow Medication Form
```
POST /appReport/borrow_medication/review_borrow_medication_form
```
**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```
**Request Body:**
```json
{
  "borrow_medication_form_id": 0,
  "cabinet_id": 0,
  "is_approved": true,
  "medication_data_list": [
    {
      "medication_id": 0,
      "borrow_amount": 0
    }
  ],
  "review_note": "string"
}
```
**Frontend Usage:** `loan-medicines.vue`

---

#### Return Medication
```
POST /appReport/borrow_medication/return_medication
```
**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```
**Request Body:**
```json
{
  "borrow_medication_form_id": 0,
  "cabinet_id": 0
}
```
**Frontend Usage:** `loan-medicines.vue`

---

### 7. User Management APIs

#### Get User List
```
GET /appReport/userList
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
roleCode: string
userName: string
currentPage: number
numberPerPage: number
sortColumn: string
sortType: asc|desc
```
**Frontend Usage:** `account-list.vue`

---

#### Get Single User
```
GET /appReport/getUser
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
id: number
```
**Frontend Usage:** `account-list.vue`

---

#### Create User
```
POST /appReport/createUser
```
**Headers:**
```
Authorization: Bearer {token}
Content-Type: multipart/form-data
```
**Request Body (FormData):**
```
photo: File
id: 0
name: string
cardCode: string
account: string
password: string
cabinetRoleId: number
functionRoleId: number
cabinetId: number (optional for super admin)
hospitalArea: string
department: string
email: string
active: boolean
```
**Frontend Usage:** `account-settings.vue`

---

#### Update User
```
POST /appReport/updateUser
```
**Headers:**
```
Authorization: Bearer {token}
Content-Type: multipart/form-data
```
**Request Body (FormData):**
Same as createUser
**Frontend Usage:** `account-settings.vue`

---

#### Delete User
```
DELETE /appReport/deleteUser/{id}
```
**Headers:**
```
Authorization: Bearer {token}
```
**Frontend Usage:** `account-list.vue`

---

#### Get Role List
```
GET /appReport/roleList
```
**Headers:**
```
Authorization: Bearer {token}
```
**Frontend Usage:** `account-list.vue`

---

### 8. Prescription Approval APIs

#### List Prescription Approvals
```
GET /appReport/appReport_api/list_prescription_approval
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
approvalStatus: W|Y|null
page: number
pagecount: number
start_date: string
end_date: string
```
**Frontend Usage:** `prescription-list.vue`

---

#### Get Prescription Approval
```
GET /appReport/appReport_api/get_prescription_approval
```
**Headers:**
```
Authorization: Bearer {token}
```
**Query Parameters:**
```
prescriptionId: number
```
**Frontend Usage:** `prescription-list.vue`, `prescription-detail.vue`

---

#### Update Prescription Approval Status
```
POST /appReport/appReport_api/update_prescription_approval_status
```
**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```
**Request Body:**
```json
{
  "approvalStatus": "Y|D|P|W",
  "approvalTime": "datetime",
  "approvalUserAccount": "string",
  "approvalUserName": "string",
  "prescriptionDetailNumber": "string"
}
```
**Status Values:**
- `Y` = Approved
- `D` = Denied/Rejected
- `P` = Pending
- `W` = Waiting for review

**Frontend Usage:** `prescription-detail.vue`

---

## Common Request/Response Patterns

### Standard Response Format
```json
{
  "success": true|false,
  "sysCode": "string",
  "message": "string",
  "data": { ... }
}
```

### Pagination Parameters
Most list endpoints support pagination:
```
currentPage: number
numberPerPage: number
sortColumn: string
sortType: "asc"|"desc"
```

### Date Range Parameters
```
startDate: "YYYY-MM-DD"
endDate: "YYYY-MM-DD"
```

### Common Error Codes
- `9999`: General error with message
- `401`: Token expired/unauthorized
- `500`: Server error
- `502`: API error

---

## WebSocket Connections

**No WebSocket connections found** in the imadc-center-v2 frontend. All communication is done via HTTP REST APIs using Axios.

---

## Files Analyzed

### Core API Configuration
- `/src/api/api.js` - Main API endpoint definitions
- `/public/static/config_report.js` - Base URL configuration

### Authentication & Session
- `/src/views/login.vue`
- `/src/views/clear-login.js`
- `/src/views/error-token.js`
- `/src/App.vue`

### Views with API Calls
- `/src/views/account/account-list.vue`
- `/src/views/account/account-settings.vue`
- `/src/views/inventory/drug-inventory.vue`
- `/src/views/inventory/inventory-stock-medication.vue`
- `/src/views/loan/loan-medicines.vue`
- `/src/views/loan/application-details.vue`
- `/src/views/prescription/prescription-list.vue`
- `/src/views/prescription/prescription-detail.vue`
- `/src/views/standing/standing-table.vue`
- `/src/views/standing/call-outInitiate.vue`
- `/src/views/standing/standing-quantity.vue`
- `/src/views/management/*.vue`
- `/src/views/report/*.vue`
- `/src/views/query/*.vue`
- `/src/views/operation/*.vue`
- `/src/views/print/*.vue`
- `/src/views/abnormal/*.vue`
- `/src/views/maintain/*.vue`
- `/src/views/record/*.vue`
- `/src/views/stock/*.vue`

---

## Export/Print API Endpoints

The following endpoints return Excel files (xlsx) for download:

| Export Endpoint | Description |
|-----------------|-------------|
| `/appReport/Reportstorage_api/export_all_cabinet_stock` | Export all cabinet stock |
| `/appReport/Reportstorage_api/getLowMedicationListReport` | Export low medication |
| `/appReport/Reportstorage_api/getCloseExpiryMediactionReport` | Export near-expiry |
| `/appReport/Reportstorage_api/get_findMediaction_excel` | Export inventory |
| `/appReport/incomeAndExpensesReportExport` | Export income/expenses |
| `/appReport/appReport_api/printDailyAverageAmountReport` | Export daily average |
| `/appReport/report_api/printMedicationStatementReport` | Export controlled drug statement |
| `/appReport/appReport_api/printBottleNoReturnReport` | Export bottle report |
| `/appReport/appReport_api/print_prescription_difference_report` | Export prescription diff |
| `/appReport/appReport_api/print_inventory_difference_report` | Export inventory diff |
| `/appReport/report_api/printSystemDifferenceReport` | Export system diff |
| `/appReport/appReport_api/printdestroyMedicationReport` | Export destroy report |
| `/appReport/Reportstorage_api/remote_call_print_standby_detail_query` | Export standby query |
| `/appReport/appReport_api/print_system_operation_report` | Export system operation |
| `/appReport/appReport_api/printExceptionReport` | Export exception report |
| `/appReport/appReport_api/printMaintenanceReport` | Export maintenance report |
| `/appReport/appReport_api/printExceptionHistoryAlertReport` | Export exception history |
| `/appReport/appReport_api/print_overall_report` | Export overall report |
| `/appReport/appReport_api/print_medicine_depot_report` | Export depot report |
| `/appReport/appReport_api/print_confine_cabinet_report` | Export confined report |
| `/appReport/cabinetInventoryReportExport` | Export cabinet inventory |
| `/appReport/appReport_api/print_cabinet_drawer_report` | Export drawer report |
| `/appReport/dateBalanceReportExport` | Export date balance |
