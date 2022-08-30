build:
	dotnet clean
	dotnet restore
	dotnet build

create-infra:
	cd iac; \
	terraform apply --auto-approve

destroy-infra:
	cd iac; \
	terraform destroy --auto-approve