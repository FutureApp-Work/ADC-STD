# iMADC Setup v2.3 Frontend API Specification

## Overview

This document contains all API endpoints called by the imadc-setup-v2-3 frontend application.

**Base URL Configuration:**
- Configured in: `/public/static/config_setup.js`
- Default: `http://127.0.0.1`
- Accessed via: `window.g.baseURL`

**Authentication:**
- Type: Bearer Token (JWT)
- Header: `Authorization: Bearer <token>`
- Token stored in: `sessionStorage.getItem("nowToken")`

**HTTP Client:**
- Library: Axios
- Global config: `axios.defaults.baseURL = window.g.baseURL`

---

## API Endpoint Summary Table

| Endpoint | Method | Frontend Usage Location | Description |
|----------|--------|------------------------|-------------|
| `/app001/getSetting` | GET | `src/get-config.js` | Get system config settings |
| `/app001/login` | POST | `src/components/login/login-account-content.vue` | User login |
| `/app001/loginUseAccountOnly` | POST | `src/App.vue` | SSO login with account only |
| `/app001/getUserList` | GET | `src/views/account/account-list.vue` | Get user account list |
| `/app001/getUser` | GET | `src/views/account/account-edit.vue` | Get single user details |
| `/app001/getRoleList` | GET | `src/views/account/account-edit.vue` | Get role list |
| `/app001/getPermissionList` | GET | `src/views/account/account-edit.vue`, `src/views/cabinet/cabinet-setup.vue` | Get permission list |
| `/app001/updateUser` | POST | `src/views/account/account-edit.vue` | Create/Update user |
| `/app001/deleteUser` | DELETE | `src/views/account/account-list.vue`, `src/views/account/account-edit.vue` | Delete user |
| `/app001/importUserCsv` | POST | `src/views/drug/drug-csv-input.vue` | Import users from CSV |
| `/app001/changePassword` | POST | `src/components/login/login-account-content.vue` | Change password |
| `/app002/setting_api/get_MasterCabinetInfo` | GET | `src/views/cabinet/cabinet-setup.vue` | Get master cabinet info |
| `/app002/setting_api/set_Cabinet` | POST | `src/views/cabinet/cabinet-setup.vue` | Update cabinet settings |
| `/app002/setting_api/get_SlaveCabinetInfo` | GET | `src/views/cabinet/cabinet-attachment.vue` | Get slave cabinet info |
| `/app002/setting_api/get_DrawerBox_list` | POST | `src/views/cabinet/cabinet-box-list.vue` | Get drawer/box list |
| `/app002/setting_api/get_drawerBoxDetail` | GET | `src/views/cabinet/cabinet-box-edit.vue` | Get box details |
| `/app002/setting_api/get_dropList` | POST | `src/views/main-theme.vue`, `src/views/settings/notification-list.vue` | Get dropdown lists |
| `/app002/setting_api/setDrawerBox` | POST | `src/views/cabinet/cabinet-box-edit.vue` | Set drawer box (initial) |
| `/app002/setting_api/update_drawerBox` | POST | `src/views/cabinet/cabinet-box-edit.vue` | Update drawer box |
| `/app002/setting_api/delete_storage` | DELETE | `src/views/cabinet/cabinet-box-edit.vue` | Delete storage item |
| `/app002/setting_api/get_Medication` | POST | `src/views/drug/drugs-list.vue` | Get medication list |
| `/app002/setting_api/getMedicationName` | GET | `src/views/cabinet/cabinet-box-edit.vue` | Get medication names (search) |
| `/app002/setting_api/update_Medication` | POST | `src/views/drug/drug-edit.vue` | Update medication |
| `/app002/setting_api/add_Medication` | POST | `src/views/drug/drug-edit.vue` | Add new medication |
| `/app002/setting_api/get_MedicationDetail` | GET | `src/views/drug/drug-edit.vue` | Get medication details |
| `/app002/setting_api/add_Medication_CSV` | POST | `src/views/drug/drug-csv-input.vue` | Import medications from CSV |
| `/app002/setting_api/del_Medication` | POST | `src/views/drug/drugs-list.vue` | Delete medication(s) |
| `/app002/setting_api/get_Informer` | POST | `src/views/settings/notification-list.vue` | Get notification person list |
| `/app002/setting_api/seach_Informer` | GET | `src/views/settings/notification-list.vue` | Search notification persons |
| `/app002/setting_api/add_Informer` | POST | `src/views/settings/notification-list.vue` | Add notification person |
| `/app002/setting_api/del_Informer` | POST | `src/views/settings/notification-list.vue` | Delete notification person(s) |
| `/app002/setting_api/getDrawer_and_Box` | GET | `src/views/settings/maintenance-list.vue` | Get drawers and boxes |
| `/app002/setting_api/get_boxMaintenanceHistoryList` | POST | `src/views/settings/maintenance-list.vue` | Get box maintenance history |
| `/app002/setting_api/get_drawerMaintenanceHistoryList` | POST | `src/views/settings/maintenance-list.vue` | Get drawer maintenance history |
| `/app002/setting_api/maintenanceElectronicLock` | POST | `src/views/settings/maintenance-list.vue` | Submit maintenance record |
| `/app002/setting_api/get_supply_source_list` | GET | `src/views/settings/source-list.vue`, `src/views/cabinet/cabinet-setup.vue` | Get supply source list |
| `/app002/setting_api/create_supply_source` | POST | `src/views/settings/source-list.vue` | Create supply source |
| `/app002/setting_api/update_supply_source` | POST | `src/views/settings/source-list.vue` | Update supply source |
| `/app002/setting_api/delete_supply_source` | DELETE | `src/views/settings/source-list.vue` | Delete supply source(s) |
| `/app002/inventory_api/get_cabinet` | GET | `src/views/cabinet/choose-cabinets.vue`, `src/views/cabinet/cabinet-box-list.vue` | Get cabinet list |
| `/app002/inventory_api/get_drug_info` | GET | Not found in scanned files | Get drug info |
| `/app002/setting_api/get_medication_excel` | GET | `src/views/drug/drugs-list.vue` | Export medications to Excel |

---

## External/Hardcoded URLs

| URL | Purpose | Location |
|-----|---------|----------|
| `http://websso.intra.tpech.gov.tw:8080/tpechSSO/LoginPage.sso` | SSO Login URL | `src/App.vue` (localStorage: `sso_url`) |
| `http://127.0.0.1:10001/SSOReceive/Setup` | SSO Callback URL | `src/App.vue` (localStorage: `sso_callback`) |
| `http://127.0.0.1:10001/CloseWindow` | Close Window URL | `src/App.vue`, `src/clear-login.js` (localStorage: `closeWindowUrl`) |
| `http://GatewayIP:10001/gateway/GetMedInfo` | Get medication info from hospital | `src/App.vue` (localStorage: `GetMedInfo`) |

---

## Authentication & Token Handling

### Token Storage
- **Token:** `sessionStorage.getItem("nowToken")`
- **Username:** `sessionStorage.getItem("userName")`
- **Idle Timeout:** `sessionStorage.getItem("idleTimeout")` (in seconds)

### Token Error Handling
**File:** `src/error-token.js`

Handles API errors:
- `401 Unauthorized`: Token expired, calls `ClearLogin()`
- `502` / `500`: API errors, shows warning message

### Clear Login Function
**File:** `src/clear-login.js`

- Clears `sessionStorage`
- If SSO enabled, calls `closeWindowUrl` with current URI
- Otherwise reloads the page

---

## Request/Response Patterns

### Standard Success Response
```json
{
  "success": true,
  "data": { ... },
  "message": "string"
}
```

### Standard Error Response
```json
{
  "success": false,
  "sysCode": "string",
  "message": "string"
}
```

Common sysCodes:
- `9999`: General error with message
- Other codes mapped to i18n keys in `SysCode.*`

---

## LocalStorage Configuration Keys

Set during initialization in `src/App.vue`:

| Key | Default | Description |
|-----|---------|-------------|
| `showDrugsCsv` | "Y" | Show CSV import button |
| `showDrugsEdit` | "Y" | Show drug add/delete buttons |
| `drugSetId` | "1" | ID for medicine sets |
| `nearExpiryDay` | "180" | Default near-expiry warning days |
| `sso` | "false" | Enable SSO login |
| `sso_url` | URL | SSO login page URL |
| `sso_callback` | URL | SSO callback URL |
| `closeWindowUrl` | URL | Window close URL for SSO |
| `showGetInfo` | "true" | Show fetch hospital drug info button |
| `GetMedInfo` | URL | Hospital drug info endpoint |
| `setup_language` | "en" | UI language (tw/en/jp) |

---

## WebSocket Connections

**No WebSocket connections found** in the analyzed codebase.

---

## API Service Structure

### Main API Definition
**File:** `src/api/api.js`

All API endpoints are defined as a single exported object:
```javascript
export const api = {
  getSetting: '/app001/getSetting?project=front-end&module=setup',
  login: 'app001/login',
  // ... other endpoints
}
```

### HTTP Helper Functions
**File:** `src/composables/callApi.js`

Basic axios wrappers:
- `apiPost(currApi, data, errorMessage)` - POST requests
- `apiGet(currApi, errorMessage)` - GET requests

### Axios Global Configuration
**File:** `src/main.js`

```javascript
import Axios from 'axios'
app.config.globalProperties.$http = Axios
```

### Axios Base URL Setup
**File:** `src/api/api.js`

```javascript
const baseURL = window.g.baseURL;
axios.defaults.baseURL = baseURL;
```

---

## Common Data Types

### Permission Codes (Cabinet Operations)
- `CabinetLoginMethod`: AccountLogin, CardLogin
- `CabinetGetAction`: PatientGet, PrescriptionGet, MedicationGet, etc.
- `CabinetReturnAction`: PatientReturn, PrescriptionReturn, MedicationReturn
- `CabinetSupply`: DrawerSupply, MedicationTypeSupply, etc.
- `CabinetInventory`: CommonlyusedInventory, DrawerInventory, etc.
- `CabinetRecall`: PatientRecall, PrescriptionRecall, MedicationRecall
- `CabinetBorrow`: ApplyBorrowMedication, BorrowMedicationReplyNotify
- `CabinetCommonAmount`: CommonAmountNotify

### Medicine Categories
- `Hoble` - High-value
- `HighAlert` - High alert
- `Regulation` - Regulated
- `LASA` - Look-alike sound-alike

### Regulatory Levels
- `Level1` - Level 1 controlled
- `Level2` - Level 2 controlled
- `Level3` - Level 3 controlled
- `Level4` - Level 4 controlled
- `General` - General medication

### Drawer Box Types
- `Get` - Retrieval box
- `Return` - Return box
- `Recall` - Recall box

### Notification Categories
- `LowSafeStock` - Low stock alert
- `ExceptionNotice` - Exception notification
- `CloseExpirationAlert` - Near expiry alert
- `RecallBlindInventory` - Recall blind inventory
- `NotDestroyAlert` - Not destroyed alert
- `BottleControlDrugSupply` - Bottle control drug supply
- `OffsetStockAlert` - Offset stock alert
- `GetBlindInventory` - Get blind inventory
- `AllCabinetStockNotice` - All cabinet stock notice

---

## Summary Statistics

- **Total API Endpoints:** 40+
- **GET Requests:** 15
- **POST Requests:** 20
- **DELETE Requests:** 4
- **External URLs:** 4
- **WebSocket Connections:** 0

---

## File Structure

```
src/
├── api/
│   └── api.js              # API endpoint definitions
├── composables/
│   └── callApi.js          # HTTP helper functions
├── views/
│   ├── account/
│   │   ├── account-list.vue
│   │   └── account-edit.vue
│   ├── cabinet/
│   │   ├── cabinet-setup.vue
│   │   ├── cabinet-attachment.vue
│   │   ├── cabinet-box-list.vue
│   │   ├── cabinet-box-edit.vue
│   │   └── choose-cabinets.vue
│   ├── drug/
│   │   ├── drugs-list.vue
│   │   ├── drug-edit.vue
│   │   └── drug-csv-input.vue
│   └── settings/
│       ├── notification-list.vue
│       ├── maintenance-list.vue
│       └── source-list.vue
├── components/
│   └── login/
│       └── login-account-content.vue
├── App.vue
├── main.js
├── get-config.js
├── error-token.js
└── clear-login.js
```
