!!!! Uncomment Ignored lines in FlightServiceContext before creating a migration

add-migration initial -context FlightServiceContext -o EntityFramework/Migrations

update-database -context FlightServiceContext

remove-migration -context FlightServiceContext 