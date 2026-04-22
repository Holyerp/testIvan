# Backlog - Monorepo Example (Backend + Mobile)

**Project:** E-Commerce Mobile App
**Structure:** Monorepo (Backend + Mobile)
**Last Updated:** 2026-04-14

---

## 📖 Prefix Legend

Use these prefixes to indicate which part of the monorepo each story affects:

- **[BE]** - Backend only (apps/backend/)
- **[Mobile]** - Mobile app only (apps/mobile/)
- **[Shared]** - Shared packages (packages/*)
- **[Full-stack]** - Touches multiple apps (Backend + Mobile coordination)

---

## Epic 1: User Authentication & Onboarding

**Priority:** P0 (Must Have)
**Total Story Points:** 34

### Stories:

- **US-001**: [BE] Implement JWT authentication API
  - **Story Points:** 5
  - **Priority:** P0
  - **Description:** Create POST /api/auth/login and /api/auth/register endpoints with JWT token generation
  - **Acceptance Criteria:**
    - Returns JWT token on successful login
    - Validates email/password format
    - Returns 401 on invalid credentials
    - Hash passwords with bcrypt

- **US-002**: [Shared] Define User and Auth TypeScript types
  - **Story Points:** 2
  - **Priority:** P0
  - **Description:** Create shared TypeScript interfaces for User, LoginRequest, AuthResponse in packages/shared-types
  - **Acceptance Criteria:**
    - User interface with id, email, name, createdAt
    - LoginRequest with email, password
    - AuthResponse with token, user, expiresAt
    - Exported from packages/shared-types/src/index.ts

- **US-003**: [Shared] Create API client wrapper
  - **Story Points:** 3
  - **Priority:** P0
  - **Description:** Build ApiClient class in packages/api-client with authentication methods
  - **Acceptance Criteria:**
    - login(email, password) method
    - register(email, password, name) method
    - Stores JWT token in AsyncStorage
    - Adds Authorization header to all requests

- **US-004**: [Mobile] Create login screen UI
  - **Story Points:** 5
  - **Priority:** P0
  - **Description:** Build login screen with email/password inputs and validation
  - **Acceptance Criteria:**
    - Email and password input fields
    - Form validation (email format, min password length)
    - Loading state during API call
    - Error message display
    - "Forgot password?" link

- **US-005**: [Mobile] Create registration screen UI
  - **Story Points:** 5
  - **Priority:** P0
  - **Description:** Build registration screen with name, email, password inputs
  - **Acceptance Criteria:**
    - Name, email, password, confirm password fields
    - Password strength indicator
    - Terms & conditions checkbox
    - Form validation
    - Navigation to login after success

- **US-006**: [Mobile] Implement biometric authentication
  - **Story Points:** 8
  - **Priority:** P1
  - **Description:** Add Face ID / Touch ID support for returning users
  - **Acceptance Criteria:**
    - Detect device biometric capability
    - Prompt user to enable biometric on first login
    - Store credentials securely (Keychain/Keystore)
    - Biometric prompt on app launch
    - Fallback to password if biometric fails

- **US-007**: [Full-stack] Social login (Google/Apple)
  - **Story Points:** 13
  - **Priority:** P1
  - **Description:** Implement OAuth login with Google and Apple Sign-In
  - **Acceptance Criteria:**
    - [BE] OAuth callback endpoints for Google/Apple
    - [BE] User account linking/creation
    - [Mobile] Google Sign-In button
    - [Mobile] Apple Sign-In button
    - [Shared] SocialAuthRequest/Response types

---

## Epic 2: Product Catalog & Discovery

**Priority:** P0 (Must Have)
**Total Story Points:** 42

### Stories:

- **US-008**: [BE] Products CRUD API endpoints
  - **Story Points:** 8
  - **Priority:** P0
  - **Description:** Create RESTful endpoints for products management
  - **Acceptance Criteria:**
    - GET /api/products (list with pagination)
    - GET /api/products/:id (single product)
    - POST /api/products (admin only)
    - PUT /api/products/:id (admin only)
    - DELETE /api/products/:id (admin only)
    - Filter by category, price range, search query

- **US-009**: [BE] Product image upload & storage
  - **Story Points:** 5
  - **Priority:** P0
  - **Description:** Implement image upload to AWS S3 or Cloudinary
  - **Acceptance Criteria:**
    - POST /api/products/:id/images endpoint
    - Resize images (thumbnail, medium, large)
    - Store URLs in database
    - Delete old images when updating

- **US-010**: [Shared] Product and Category TypeScript types
  - **Story Points:** 3
  - **Priority:** P0
  - **Description:** Define Product, Category, ProductImage interfaces
  - **Acceptance Criteria:**
    - Product with id, name, description, price, images[], categoryId
    - Category with id, name, slug, parentId
    - ProductImage with id, url, size, type

- **US-011**: [Shared] Extend API client with product methods
  - **Story Points:** 3
  - **Priority:** P0
  - **Description:** Add product API methods to packages/api-client
  - **Acceptance Criteria:**
    - getProducts(filters) method
    - getProduct(id) method
    - searchProducts(query) method

- **US-012**: [Mobile] Product listing screen with grid
  - **Story Points:** 8
  - **Priority:** P0
  - **Description:** Display products in scrollable grid with images
  - **Acceptance Criteria:**
    - 2-column grid layout
    - Product image, name, price display
    - Pull-to-refresh functionality
    - Infinite scroll / pagination
    - Loading skeleton UI

- **US-013**: [Mobile] Product detail screen
  - **Story Points:** 5
  - **Priority:** P0
  - **Description:** Full product details with image gallery
  - **Acceptance Criteria:**
    - Image carousel (swipeable)
    - Product name, description, price
    - "Add to cart" button
    - Quantity selector
    - Share product button

- **US-014**: [Mobile] Search functionality with filters
  - **Story Points:** 8
  - **Priority:** P0
  - **Description:** Search bar with category and price filters
  - **Acceptance Criteria:**
    - Search input with autocomplete
    - Category filter dropdown
    - Price range slider
    - Sort options (price, name, popularity)
    - Results count display

- **US-015**: [Mobile] Product favorites/wishlist
  - **Story Points:** 5
  - **Priority:** P1
  - **Description:** Allow users to save products to favorites
  - **Acceptance Criteria:**
    - Heart icon on product cards
    - Favorites screen showing saved products
    - Sync favorites to backend
    - Remove from favorites option

---

## Epic 3: Shopping Cart & Checkout

**Priority:** P0 (Must Have)
**Total Story Points:** 47

### Stories:

- **US-016**: [BE] Shopping cart session management
  - **Story Points:** 8
  - **Priority:** P0
  - **Description:** Backend cart storage with guest and authenticated users
  - **Acceptance Criteria:**
    - POST /api/cart/items (add to cart)
    - GET /api/cart (get cart)
    - PUT /api/cart/items/:id (update quantity)
    - DELETE /api/cart/items/:id (remove item)
    - Cart persists for authenticated users
    - Session cart for guest users

- **US-017**: [Shared] Cart and CartItem TypeScript types
  - **Story Points:** 2
  - **Priority:** P0
  - **Description:** Define cart-related interfaces
  - **Acceptance Criteria:**
    - Cart with id, userId, items[], total, createdAt
    - CartItem with id, productId, quantity, price
    - CartSummary with subtotal, tax, shipping, total

- **US-018**: [Shared] Extend API client with cart methods
  - **Story Points:** 3
  - **Priority:** P0
  - **Description:** Cart management methods in api-client
  - **Acceptance Criteria:**
    - addToCart(productId, quantity) method
    - getCart() method
    - updateCartItem(itemId, quantity) method
    - removeFromCart(itemId) method

- **US-019**: [Mobile] Shopping cart UI with item management
  - **Story Points:** 8
  - **Priority:** P0
  - **Description:** Cart screen with quantity controls
  - **Acceptance Criteria:**
    - List of cart items with images
    - Quantity +/- buttons
    - Remove item swipe gesture
    - Subtotal calculation
    - "Proceed to checkout" button

- **US-020**: [Full-stack] Real-time cart sync across devices
  - **Story Points:** 13
  - **Priority:** P1
  - **Description:** Sync cart changes instantly when user logs in on another device
  - **Acceptance Criteria:**
    - [BE] WebSocket or polling endpoint for cart updates
    - [Mobile] Listen for cart changes
    - [Mobile] Merge guest cart with user cart on login
    - [Shared] CartSyncEvent types
    - Optimistic UI updates

- **US-021**: [BE] Payment processing integration (Stripe)
  - **Story Points:** 13
  - **Priority:** P0
  - **Description:** Integrate Stripe for payment processing
  - **Acceptance Criteria:**
    - POST /api/checkout/create-payment-intent
    - POST /api/checkout/confirm-payment
    - Webhook for payment events
    - Store order after successful payment
    - Deduct inventory after payment

- **US-022**: [Mobile] Checkout flow with Stripe SDK
  - **Story Points:** 8
  - **Priority:** P0
  - **Description:** Multi-step checkout with payment
  - **Acceptance Criteria:**
    - Shipping address form
    - Payment method selection
    - Order summary review
    - Stripe payment sheet integration
    - Order confirmation screen

---

## Epic 4: User Profile & Orders

**Priority:** P1 (Should Have)
**Total Story Points:** 29

### Stories:

- **US-023**: [BE] User profile CRUD API
  - **Story Points:** 5
  - **Priority:** P1
  - **Description:** User profile management endpoints
  - **Acceptance Criteria:**
    - GET /api/users/me (current user)
    - PUT /api/users/me (update profile)
    - PUT /api/users/me/password (change password)
    - Upload profile picture

- **US-024**: [BE] Order history API
  - **Story Points:** 5
  - **Priority:** P1
  - **Description:** Retrieve user's past orders
  - **Acceptance Criteria:**
    - GET /api/orders (list with pagination)
    - GET /api/orders/:id (order details)
    - Filter by status, date range

- **US-025**: [Mobile] Profile screen
  - **Story Points:** 5
  - **Priority:** P1
  - **Description:** User profile view and edit
  - **Acceptance Criteria:**
    - Display name, email, profile picture
    - Edit profile button
    - Change password option
    - Logout button

- **US-026**: [Mobile] Order history screen
  - **Story Points:** 8
  - **Priority:** P1
  - **Description:** List of user's orders with status
  - **Acceptance Criteria:**
    - Order list sorted by date (newest first)
    - Order status badges (pending, shipped, delivered)
    - Tap order to view details
    - Track shipment button (if applicable)

- **US-027**: [Mobile] Order details screen
  - **Story Points:** 5
  - **Priority:** P1
  - **Description:** Full order information
  - **Acceptance Criteria:**
    - Order items list with images
    - Order total breakdown
    - Shipping address
    - Order status timeline
    - Reorder button

---

## Epic 5: Push Notifications

**Priority:** P2 (Nice to Have)
**Total Story Points:** 18

### Stories:

- **US-028**: [BE] Push notification service setup
  - **Story Points:** 8
  - **Priority:** P2
  - **Description:** Firebase Cloud Messaging integration
  - **Acceptance Criteria:**
    - Store device FCM tokens
    - Send notification API endpoint
    - Handle token refresh
    - Notification templates

- **US-029**: [Mobile] Push notification permissions & handling
  - **Story Points:** 5
  - **Priority:** P2
  - **Description:** Request push permissions and handle notifications
  - **Acceptance Criteria:**
    - Request permission on app launch (after login)
    - Register FCM token with backend
    - Handle foreground notifications
    - Handle background notifications
    - Navigate to relevant screen on notification tap

- **US-030**: [Full-stack] Order status notifications
  - **Story Points:** 5
  - **Priority:** P2
  - **Description:** Send push when order status changes
  - **Acceptance Criteria:**
    - [BE] Trigger notification on order update
    - [Mobile] Display notification
    - [Mobile] Deep link to order details
    - Notification preferences in settings

---

## Backlog Future (Post-Launch v2.0+)

### Epic 6: Advanced Features
- US-031: [Mobile] Dark mode support
- US-032: [Full-stack] Live chat support
- US-033: [Mobile] Offline mode with local cache
- US-034: [Full-stack] Product recommendations ML

### Epic 7: Admin Features
- US-035: [Web] Admin dashboard (new app in monorepo)
- US-036: [BE] Admin analytics API
- US-037: [BE] Inventory management

---

## Summary Statistics

**Total Epics (Active):** 5
**Total Stories (Active):** 30
**Total Story Points:** 170

**By Priority:**
- P0 (Must Have): 22 stories, 127 points
- P1 (Should Have): 5 stories, 29 points
- P2 (Nice to Have): 3 stories, 18 points

**By Component:**
- [BE]: 9 stories
- [Mobile]: 13 stories
- [Shared]: 5 stories
- [Full-stack]: 3 stories

---

**Note:** This backlog demonstrates monorepo structure with clear separation of backend, mobile, and shared code concerns.
