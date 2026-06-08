using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiberus.Industria.Vektor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SnakeCaseAndAuditingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Tenants_TenantId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tenants_TenantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Tenants_TenantId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "ActiveVehicleRoutes");

            migrationBuilder.DropTable(
                name: "DriverVehicleAssignments");

            migrationBuilder.DropTable(
                name: "OrderAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drivers",
                table: "Drivers");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "vehicles");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Tenants",
                newName: "tenants");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "orders");

            migrationBuilder.RenameTable(
                name: "Drivers",
                newName: "drivers");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "vehicles",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "vehicles",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "vehicles",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Speed",
                table: "vehicles",
                newName: "speed");

            migrationBuilder.RenameColumn(
                name: "Rpm",
                table: "vehicles",
                newName: "rpm");

            migrationBuilder.RenameColumn(
                name: "Odometer",
                table: "vehicles",
                newName: "odometer");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "vehicles",
                newName: "model");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "vehicles",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "vehicles",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "vehicles",
                newName: "label");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "vehicles",
                newName: "brand");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vehicles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "vehicles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "vehicles",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "vehicles",
                newName: "license_plate");

            migrationBuilder.RenameColumn(
                name: "LastGpsUpdate",
                table: "vehicles",
                newName: "last_gps_update");

            migrationBuilder.RenameColumn(
                name: "FuelLevel",
                table: "vehicles",
                newName: "fuel_level");

            migrationBuilder.RenameColumn(
                name: "EngineTemp",
                table: "vehicles",
                newName: "engine_temp");

            migrationBuilder.RenameColumn(
                name: "DtcCodes",
                table: "vehicles",
                newName: "dtc_codes");

            migrationBuilder.RenameColumn(
                name: "DeviceImei",
                table: "vehicles",
                newName: "device_imei");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "vehicles",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BatteryLevel",
                table: "vehicles",
                newName: "battery_level");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_TenantId",
                table: "vehicles",
                newName: "IX_vehicles_tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_DeviceImei",
                table: "vehicles",
                newName: "IX_vehicles_device_imei");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "users",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "users",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Users_TenantId",
                table: "users",
                newName: "IX_users_tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email_TenantId",
                table: "users",
                newName: "IX_users_email_tenant_id");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "tenants",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "tenants",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tenants",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "tenants",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "tenants",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Tenants_Slug",
                table: "tenants",
                newName: "IX_tenants_slug");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "orders",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "orders",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "orders",
                newName: "label");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "orders",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "orders",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "ExternalOrderId",
                table: "orders",
                newName: "external_order_id");

            migrationBuilder.RenameColumn(
                name: "CustomerPhone",
                table: "orders",
                newName: "customer_phone");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "orders",
                newName: "customer_name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "orders",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Status",
                table: "orders",
                newName: "IX_orders_status");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_TenantId",
                table: "orders",
                newName: "IX_orders_tenant_id");

            migrationBuilder.RenameColumn(
                name: "Timezone",
                table: "drivers",
                newName: "timezone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "drivers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "drivers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkdayStartTime",
                table: "drivers",
                newName: "workday_start_time");

            migrationBuilder.RenameColumn(
                name: "WorkdayEndTime",
                table: "drivers",
                newName: "workday_end_time");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "drivers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "drivers",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "drivers",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "LicenseType",
                table: "drivers",
                newName: "license_type");

            migrationBuilder.RenameColumn(
                name: "LicenseNumber",
                table: "drivers",
                newName: "license_number");

            migrationBuilder.RenameColumn(
                name: "LicenseExpiryDate",
                table: "drivers",
                newName: "license_expiry_date");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "drivers",
                newName: "is_available");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "drivers",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "drivers",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_TenantId",
                table: "drivers",
                newName: "IX_drivers_tenant_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "vehicles",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "vehicles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "vehicles",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "tenants",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tenants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "orders",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "drivers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "drivers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "drivers",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tenants",
                table: "tenants",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                table: "orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drivers",
                table: "drivers",
                column: "id");

            migrationBuilder.CreateTable(
                name: "active_vehicle_routes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    route_payload = table.Column<string>(type: "text", nullable: false),
                    associated_order_ids = table.Column<string>(type: "text", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_active_vehicle_routes", x => x.id);
                    table.ForeignKey(
                        name: "FK_active_vehicle_routes_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_vehicle_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    unassigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver_vehicle_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_driver_vehicle_assignments_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_driver_vehicle_assignments_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    actual_arrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failure_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_assignments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_assignments_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_active_vehicle_routes_tenant_id",
                table: "active_vehicle_routes",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_active_vehicle_routes_vehicle_id",
                table: "active_vehicle_routes",
                column: "vehicle_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_driver_vehicle_assignments_driver_id_assigned_at",
                table: "driver_vehicle_assignments",
                columns: new[] { "driver_id", "assigned_at" });

            migrationBuilder.CreateIndex(
                name: "IX_driver_vehicle_assignments_vehicle_id_assigned_at",
                table: "driver_vehicle_assignments",
                columns: new[] { "vehicle_id", "assigned_at" });

            migrationBuilder.CreateIndex(
                name: "IX_order_assignments_order_id",
                table: "order_assignments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_assignments_status",
                table: "order_assignments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_order_assignments_vehicle_id",
                table: "order_assignments",
                column: "vehicle_id");

            migrationBuilder.AddForeignKey(
                name: "FK_drivers_tenants_tenant_id",
                table: "drivers",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_tenants_tenant_id",
                table: "orders",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_tenants_tenant_id",
                table: "users",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_tenants_tenant_id",
                table: "vehicles",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_drivers_tenants_tenant_id",
                table: "drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_tenants_tenant_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_users_tenants_tenant_id",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_tenants_tenant_id",
                table: "vehicles");

            migrationBuilder.DropTable(
                name: "active_vehicle_routes");

            migrationBuilder.DropTable(
                name: "driver_vehicle_assignments");

            migrationBuilder.DropTable(
                name: "order_assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tenants",
                table: "tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_drivers",
                table: "drivers");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "vehicles");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "vehicles");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tenants");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "tenants");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "drivers");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "drivers");

            migrationBuilder.RenameTable(
                name: "vehicles",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "tenants",
                newName: "Tenants");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "drivers",
                newName: "Drivers");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "Vehicles",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Vehicles",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Vehicles",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "speed",
                table: "Vehicles",
                newName: "Speed");

            migrationBuilder.RenameColumn(
                name: "rpm",
                table: "Vehicles",
                newName: "Rpm");

            migrationBuilder.RenameColumn(
                name: "odometer",
                table: "Vehicles",
                newName: "Odometer");

            migrationBuilder.RenameColumn(
                name: "model",
                table: "Vehicles",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Vehicles",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Vehicles",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "Vehicles",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "brand",
                table: "Vehicles",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Vehicles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Vehicles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Vehicles",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "license_plate",
                table: "Vehicles",
                newName: "LicensePlate");

            migrationBuilder.RenameColumn(
                name: "last_gps_update",
                table: "Vehicles",
                newName: "LastGpsUpdate");

            migrationBuilder.RenameColumn(
                name: "fuel_level",
                table: "Vehicles",
                newName: "FuelLevel");

            migrationBuilder.RenameColumn(
                name: "engine_temp",
                table: "Vehicles",
                newName: "EngineTemp");

            migrationBuilder.RenameColumn(
                name: "dtc_codes",
                table: "Vehicles",
                newName: "DtcCodes");

            migrationBuilder.RenameColumn(
                name: "device_imei",
                table: "Vehicles",
                newName: "DeviceImei");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Vehicles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "battery_level",
                table: "Vehicles",
                newName: "BatteryLevel");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_tenant_id",
                table: "Vehicles",
                newName: "IX_Vehicles_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_device_imei",
                table: "Vehicles",
                newName: "IX_Vehicles_DeviceImei");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Users",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_users_tenant_id",
                table: "Users",
                newName: "IX_Users_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_users_email_tenant_id",
                table: "Users",
                newName: "IX_Users_Email_TenantId");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Tenants",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Tenants",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tenants",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Tenants",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Tenants",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_tenants_slug",
                table: "Tenants",
                newName: "IX_Tenants_Slug");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Orders",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Orders",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "Orders",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Orders",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Orders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Orders",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "external_order_id",
                table: "Orders",
                newName: "ExternalOrderId");

            migrationBuilder.RenameColumn(
                name: "customer_phone",
                table: "Orders",
                newName: "CustomerPhone");

            migrationBuilder.RenameColumn(
                name: "customer_name",
                table: "Orders",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Orders",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_orders_status",
                table: "Orders",
                newName: "IX_Orders_Status");

            migrationBuilder.RenameIndex(
                name: "IX_orders_tenant_id",
                table: "Orders",
                newName: "IX_Orders_TenantId");

            migrationBuilder.RenameColumn(
                name: "timezone",
                table: "Drivers",
                newName: "Timezone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Drivers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Drivers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workday_start_time",
                table: "Drivers",
                newName: "WorkdayStartTime");

            migrationBuilder.RenameColumn(
                name: "workday_end_time",
                table: "Drivers",
                newName: "WorkdayEndTime");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Drivers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Drivers",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Drivers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "license_type",
                table: "Drivers",
                newName: "LicenseType");

            migrationBuilder.RenameColumn(
                name: "license_number",
                table: "Drivers",
                newName: "LicenseNumber");

            migrationBuilder.RenameColumn(
                name: "license_expiry_date",
                table: "Drivers",
                newName: "LicenseExpiryDate");

            migrationBuilder.RenameColumn(
                name: "is_available",
                table: "Drivers",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "Drivers",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Drivers",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_drivers_tenant_id",
                table: "Drivers",
                newName: "IX_Drivers_TenantId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Vehicles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tenants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Drivers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drivers",
                table: "Drivers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ActiveVehicleRoutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssociatedOrderIds = table.Column<string>(type: "text", nullable: false),
                    RoutePayload = table.Column<string>(type: "text", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveVehicleRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveVehicleRoutes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverVehicleAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnassignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverVehicleAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverVehicleAssignments_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverVehicleAssignments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActualArrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderAssignments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderAssignments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveVehicleRoutes_TenantId",
                table: "ActiveVehicleRoutes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveVehicleRoutes_VehicleId",
                table: "ActiveVehicleRoutes",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverVehicleAssignments_DriverId_AssignedAt",
                table: "DriverVehicleAssignments",
                columns: new[] { "DriverId", "AssignedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DriverVehicleAssignments_VehicleId_AssignedAt",
                table: "DriverVehicleAssignments",
                columns: new[] { "VehicleId", "AssignedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderAssignments_OrderId",
                table: "OrderAssignments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAssignments_Status",
                table: "OrderAssignments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAssignments_VehicleId",
                table: "OrderAssignments",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Tenants_TenantId",
                table: "Drivers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tenants_TenantId",
                table: "Orders",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Tenants_TenantId",
                table: "Vehicles",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
