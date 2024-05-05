﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bar",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { new Guid("01ea0ae2-5c93-8f3e-b15d-87d9a41db1db"), 4 },
                    { new Guid("05c4a72c-dae6-bec6-31f6-449848b58689"), 6 },
                    { new Guid("06c052f4-2eb3-7d01-4422-f087fb622959"), 5 },
                    { new Guid("077a5744-6f4c-1ff0-7c29-8b8e6d2020b6"), 10 },
                    { new Guid("0b5d2f9b-a469-cd00-6bdd-20d7e216f1c1"), 13 },
                    { new Guid("0d7f0397-7b42-ed7e-b0cf-3c9bc3867194"), 15 },
                    { new Guid("0e1904c5-be4f-d062-684a-314fa91a00cc"), 8 },
                    { new Guid("0e23df69-24d8-3c6b-9381-7a2e829d4b75"), 3 },
                    { new Guid("11141965-50ce-2b00-96b0-ea4d990d5d6d"), 9 },
                    { new Guid("11bc4982-6f2a-1499-e5f3-0728a89e1504"), 1 },
                    { new Guid("1298d3d5-65bd-c51c-0b7a-2db4fd90542d"), 2 },
                    { new Guid("13433da0-15ea-97d8-63ed-38108d56545d"), 15 },
                    { new Guid("193da452-0271-3c23-81df-253b3455968e"), 4 },
                    { new Guid("213b48b9-5704-1017-04e1-dc64884dfad9"), 6 },
                    { new Guid("230683a3-08f9-3afe-b3bd-bcb526270943"), 10 },
                    { new Guid("2339dc01-09e9-6b86-4fa9-6ee1a6270ea2"), 12 },
                    { new Guid("24ea76ab-49ff-afe8-b14f-d0733922bc66"), 11 },
                    { new Guid("257e8c40-8fb4-6839-881d-c28a5f29a5e9"), 6 },
                    { new Guid("28755ab2-dae0-75f6-358b-090b87f6bfce"), 11 },
                    { new Guid("2a563683-001a-befe-fed8-1334556af177"), 6 },
                    { new Guid("34c8a2f6-67a9-3e00-aa34-5e8ac8d912ac"), 8 },
                    { new Guid("37e463c3-b32a-014c-ac29-8affc81b4569"), 4 },
                    { new Guid("389701b0-105c-0ebe-8a97-ae0282fd8570"), 13 },
                    { new Guid("3b9c570d-b0db-3276-c9d2-16663ee63aab"), 14 },
                    { new Guid("3f6c1119-10fb-84a0-bba3-2a063ece8d76"), 5 },
                    { new Guid("43ef2ba7-e8b2-52f2-d992-46831d0fa6d3"), 15 },
                    { new Guid("45a39f29-73ae-3552-142e-2a6f535a0757"), 14 },
                    { new Guid("48cad340-eb80-82fb-7bf1-8e6af21b361c"), 4 },
                    { new Guid("4a6a9c4a-8b73-7d66-0d09-77fc2e420863"), 15 },
                    { new Guid("4c0f0362-2cd4-c769-aeb4-043cbdb2e3cb"), 7 },
                    { new Guid("4ef10c91-3d2b-8773-720b-7231475688de"), 7 },
                    { new Guid("4f28491f-2824-6694-e5a8-45d4c79835d6"), 7 },
                    { new Guid("506d9862-1b4e-de9e-ea4b-ba92a0fb31c0"), 13 },
                    { new Guid("51d8e208-f1fa-96b0-6034-8023b578df0c"), 14 },
                    { new Guid("57f64b41-9dad-9cdc-42c2-dbf88dfad3df"), 8 },
                    { new Guid("5a45ba7e-1478-0f1f-484c-16d8ed6b3f60"), 3 },
                    { new Guid("5af86138-52d1-500c-b1c0-2eeea149b59d"), 1 },
                    { new Guid("5b355658-f3c5-4710-77b8-baff1b59b4d3"), 14 },
                    { new Guid("5ed283bd-87de-3bf0-8cca-e510f5f51a6e"), 5 },
                    { new Guid("63a58dc5-7ac4-367c-899a-4837ddccacc3"), 12 },
                    { new Guid("66c34c56-fe52-d80a-d479-2608222c8744"), 12 },
                    { new Guid("698ee62a-7968-9cd9-ed47-886192f58c88"), 14 },
                    { new Guid("6e0d571c-5590-2891-a7a3-fa29fdd4fd41"), 7 },
                    { new Guid("724fbf89-c2c1-5b44-553a-0bd8a67bf2a3"), 4 },
                    { new Guid("75b2a36f-2462-aa12-6201-43f1627526b0"), 6 },
                    { new Guid("777f440d-db88-52b4-10ae-d359485d3431"), 4 },
                    { new Guid("78bda0ae-ca3d-cb5e-2fb1-81904000ef25"), 9 },
                    { new Guid("7bc6c4c3-1ec8-d5b2-d7a1-1e446bb40b17"), 14 },
                    { new Guid("7f49844d-791d-cb6e-057c-3e2ceffdf245"), 14 },
                    { new Guid("801bff34-66dd-692d-ac6f-5cda8cdd5af1"), 11 },
                    { new Guid("80c3cfae-8d39-820c-a508-bac217e92dfc"), 5 },
                    { new Guid("834b5393-e3f3-0d4c-425a-0f1a52c9a7b8"), 7 },
                    { new Guid("84b18847-fc52-9681-dc79-34bd13bec768"), 11 },
                    { new Guid("87b08676-9903-60ec-094b-a5ed4bc79373"), 5 },
                    { new Guid("8876d078-b3c1-0592-477b-7cc4798445e1"), 15 },
                    { new Guid("88d15964-d697-0ef3-b3cc-5e5e53be2e30"), 8 },
                    { new Guid("9193c3ca-5403-cbe0-6349-2bed611a178c"), 15 },
                    { new Guid("96b8ab4d-66cf-371c-d182-7de665418ff7"), 4 },
                    { new Guid("97447220-ded6-736a-de49-445105c865aa"), 12 },
                    { new Guid("9993a039-17d1-4ea2-708f-ddfdd2f31ae3"), 5 },
                    { new Guid("9c40b1f5-ee40-3f03-5ad7-1ad1ec822747"), 1 },
                    { new Guid("9f9c587e-2334-9fd1-1a22-d1d8c37cf088"), 12 },
                    { new Guid("9fe09dfc-15a7-a328-815a-c01b417cf9ed"), 10 },
                    { new Guid("a20b9452-b4d3-74d3-7d65-9ff8de923887"), 13 },
                    { new Guid("a5a3f510-787a-aab8-d824-7e7b1e4b6773"), 6 },
                    { new Guid("a63beb56-ea67-7468-91c0-afe776dbf1ad"), 7 },
                    { new Guid("a7db69eb-45f6-092f-8c12-f5ecbded6c80"), 11 },
                    { new Guid("a833e212-ddfb-cf1f-2e36-40ced4b70a85"), 14 },
                    { new Guid("abf5f590-ffdf-2679-b88e-686b6e663a97"), 8 },
                    { new Guid("ac45621f-9011-7cef-022f-36f50ed250fc"), 9 },
                    { new Guid("b3612457-1fc2-6949-1d64-14670e88e8df"), 13 },
                    { new Guid("bdf4180f-4b01-e4fa-c311-da0bd1813f83"), 9 },
                    { new Guid("bfcb3388-f064-fe38-8a88-8f43599d8128"), 4 },
                    { new Guid("c0a3caad-d0c7-452c-d89b-92a5252bbd68"), 3 },
                    { new Guid("c345b252-1273-2976-cf33-a772788a627d"), 13 },
                    { new Guid("c375d709-2fe8-45ff-37a0-79accabc75f2"), 11 },
                    { new Guid("c622b882-001f-536c-b2b8-109546296ec5"), 6 },
                    { new Guid("c6d42e3a-6c16-6aaf-30d5-6b22ecbd059f"), 4 },
                    { new Guid("c6f4df9e-50be-e3a9-f9cc-18d85b8cc08d"), 8 },
                    { new Guid("c937f984-8a31-52f5-0542-3fbd7b797e4d"), 6 },
                    { new Guid("d0637781-ed9e-6e85-e7f2-341b9d7c15df"), 7 },
                    { new Guid("d0c020a5-7020-a056-c4a9-3e6e3efe410d"), 6 },
                    { new Guid("d3390c66-39bd-8a3a-ca5a-cc5ec4ac57c4"), 9 },
                    { new Guid("d55708e1-68d7-10b3-3ba3-c9ace32324df"), 4 },
                    { new Guid("d5f23e04-4529-1911-56ad-2e575493c7f5"), 1 },
                    { new Guid("d800f17e-280c-cf5b-ce10-2b2a30bf4d85"), 9 },
                    { new Guid("e15b7598-bc58-34cf-f1b9-5e3f433ad8bc"), 10 },
                    { new Guid("e384f8a3-29ee-8d84-c7ce-0164ece0b38f"), 7 },
                    { new Guid("e6211ae8-1247-6824-c70a-db3c7add1596"), 14 },
                    { new Guid("eda3267b-1e3b-415c-96cd-fa2b28ad0884"), 5 },
                    { new Guid("f270bdc7-329f-52f3-61b9-4e5c5ae86531"), 10 },
                    { new Guid("f296158d-ea48-529f-ceff-1f45b47c3fcb"), 1 },
                    { new Guid("f5029edf-f2ad-3c47-1457-fd6bb75b3c01"), 8 },
                    { new Guid("f6e9da8c-7452-e110-2cf9-dc01f4c16453"), 2 },
                    { new Guid("f960fbbf-3abf-486b-4e35-2d5b1f99f0c0"), 10 },
                    { new Guid("fc38325e-639f-c332-202b-764d885ddf91"), 12 },
                    { new Guid("fc56e902-c9b7-a12e-3d7a-8379dce11702"), 1 },
                    { new Guid("fd0eb117-fe83-e95d-9f6a-ef69442b3be8"), 1 },
                    { new Guid("fde654de-90bd-2765-97f0-ac576b9ef897"), 11 },
                    { new Guid("fe75a530-3597-1a70-8e20-0ff66f223075"), 12 }
                });

            migrationBuilder.InsertData(
                table: "Foo",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { new Guid("01ea0ae2-5c93-8f3e-b15d-87d9a41db1db"), 4 },
                    { new Guid("05c4a72c-dae6-bec6-31f6-449848b58689"), 6 },
                    { new Guid("06c052f4-2eb3-7d01-4422-f087fb622959"), 5 },
                    { new Guid("077a5744-6f4c-1ff0-7c29-8b8e6d2020b6"), 10 },
                    { new Guid("0b5d2f9b-a469-cd00-6bdd-20d7e216f1c1"), 13 },
                    { new Guid("0d7f0397-7b42-ed7e-b0cf-3c9bc3867194"), 15 },
                    { new Guid("0e1904c5-be4f-d062-684a-314fa91a00cc"), 8 },
                    { new Guid("0e23df69-24d8-3c6b-9381-7a2e829d4b75"), 3 },
                    { new Guid("11141965-50ce-2b00-96b0-ea4d990d5d6d"), 9 },
                    { new Guid("11bc4982-6f2a-1499-e5f3-0728a89e1504"), 1 },
                    { new Guid("1298d3d5-65bd-c51c-0b7a-2db4fd90542d"), 2 },
                    { new Guid("13433da0-15ea-97d8-63ed-38108d56545d"), 15 },
                    { new Guid("193da452-0271-3c23-81df-253b3455968e"), 4 },
                    { new Guid("213b48b9-5704-1017-04e1-dc64884dfad9"), 6 },
                    { new Guid("230683a3-08f9-3afe-b3bd-bcb526270943"), 10 },
                    { new Guid("2339dc01-09e9-6b86-4fa9-6ee1a6270ea2"), 12 },
                    { new Guid("24ea76ab-49ff-afe8-b14f-d0733922bc66"), 11 },
                    { new Guid("257e8c40-8fb4-6839-881d-c28a5f29a5e9"), 6 },
                    { new Guid("28755ab2-dae0-75f6-358b-090b87f6bfce"), 11 },
                    { new Guid("2a563683-001a-befe-fed8-1334556af177"), 6 },
                    { new Guid("34c8a2f6-67a9-3e00-aa34-5e8ac8d912ac"), 8 },
                    { new Guid("37e463c3-b32a-014c-ac29-8affc81b4569"), 4 },
                    { new Guid("389701b0-105c-0ebe-8a97-ae0282fd8570"), 13 },
                    { new Guid("3b9c570d-b0db-3276-c9d2-16663ee63aab"), 14 },
                    { new Guid("3f6c1119-10fb-84a0-bba3-2a063ece8d76"), 5 },
                    { new Guid("43ef2ba7-e8b2-52f2-d992-46831d0fa6d3"), 15 },
                    { new Guid("45a39f29-73ae-3552-142e-2a6f535a0757"), 14 },
                    { new Guid("48cad340-eb80-82fb-7bf1-8e6af21b361c"), 4 },
                    { new Guid("4a6a9c4a-8b73-7d66-0d09-77fc2e420863"), 15 },
                    { new Guid("4c0f0362-2cd4-c769-aeb4-043cbdb2e3cb"), 7 },
                    { new Guid("4ef10c91-3d2b-8773-720b-7231475688de"), 7 },
                    { new Guid("4f28491f-2824-6694-e5a8-45d4c79835d6"), 7 },
                    { new Guid("506d9862-1b4e-de9e-ea4b-ba92a0fb31c0"), 13 },
                    { new Guid("51d8e208-f1fa-96b0-6034-8023b578df0c"), 14 },
                    { new Guid("57f64b41-9dad-9cdc-42c2-dbf88dfad3df"), 8 },
                    { new Guid("5a45ba7e-1478-0f1f-484c-16d8ed6b3f60"), 3 },
                    { new Guid("5af86138-52d1-500c-b1c0-2eeea149b59d"), 1 },
                    { new Guid("5b355658-f3c5-4710-77b8-baff1b59b4d3"), 14 },
                    { new Guid("5ed283bd-87de-3bf0-8cca-e510f5f51a6e"), 5 },
                    { new Guid("63a58dc5-7ac4-367c-899a-4837ddccacc3"), 12 },
                    { new Guid("66c34c56-fe52-d80a-d479-2608222c8744"), 12 },
                    { new Guid("698ee62a-7968-9cd9-ed47-886192f58c88"), 14 },
                    { new Guid("6e0d571c-5590-2891-a7a3-fa29fdd4fd41"), 7 },
                    { new Guid("724fbf89-c2c1-5b44-553a-0bd8a67bf2a3"), 4 },
                    { new Guid("75b2a36f-2462-aa12-6201-43f1627526b0"), 6 },
                    { new Guid("777f440d-db88-52b4-10ae-d359485d3431"), 4 },
                    { new Guid("78bda0ae-ca3d-cb5e-2fb1-81904000ef25"), 9 },
                    { new Guid("7bc6c4c3-1ec8-d5b2-d7a1-1e446bb40b17"), 14 },
                    { new Guid("7f49844d-791d-cb6e-057c-3e2ceffdf245"), 14 },
                    { new Guid("801bff34-66dd-692d-ac6f-5cda8cdd5af1"), 11 },
                    { new Guid("80c3cfae-8d39-820c-a508-bac217e92dfc"), 5 },
                    { new Guid("834b5393-e3f3-0d4c-425a-0f1a52c9a7b8"), 7 },
                    { new Guid("84b18847-fc52-9681-dc79-34bd13bec768"), 11 },
                    { new Guid("87b08676-9903-60ec-094b-a5ed4bc79373"), 5 },
                    { new Guid("8876d078-b3c1-0592-477b-7cc4798445e1"), 15 },
                    { new Guid("88d15964-d697-0ef3-b3cc-5e5e53be2e30"), 8 },
                    { new Guid("9193c3ca-5403-cbe0-6349-2bed611a178c"), 15 },
                    { new Guid("96b8ab4d-66cf-371c-d182-7de665418ff7"), 4 },
                    { new Guid("97447220-ded6-736a-de49-445105c865aa"), 12 },
                    { new Guid("9993a039-17d1-4ea2-708f-ddfdd2f31ae3"), 5 },
                    { new Guid("9c40b1f5-ee40-3f03-5ad7-1ad1ec822747"), 1 },
                    { new Guid("9f9c587e-2334-9fd1-1a22-d1d8c37cf088"), 12 },
                    { new Guid("9fe09dfc-15a7-a328-815a-c01b417cf9ed"), 10 },
                    { new Guid("a20b9452-b4d3-74d3-7d65-9ff8de923887"), 13 },
                    { new Guid("a5a3f510-787a-aab8-d824-7e7b1e4b6773"), 6 },
                    { new Guid("a63beb56-ea67-7468-91c0-afe776dbf1ad"), 7 },
                    { new Guid("a7db69eb-45f6-092f-8c12-f5ecbded6c80"), 11 },
                    { new Guid("a833e212-ddfb-cf1f-2e36-40ced4b70a85"), 14 },
                    { new Guid("abf5f590-ffdf-2679-b88e-686b6e663a97"), 8 },
                    { new Guid("ac45621f-9011-7cef-022f-36f50ed250fc"), 9 },
                    { new Guid("b3612457-1fc2-6949-1d64-14670e88e8df"), 13 },
                    { new Guid("bdf4180f-4b01-e4fa-c311-da0bd1813f83"), 9 },
                    { new Guid("bfcb3388-f064-fe38-8a88-8f43599d8128"), 4 },
                    { new Guid("c0a3caad-d0c7-452c-d89b-92a5252bbd68"), 3 },
                    { new Guid("c345b252-1273-2976-cf33-a772788a627d"), 13 },
                    { new Guid("c375d709-2fe8-45ff-37a0-79accabc75f2"), 11 },
                    { new Guid("c622b882-001f-536c-b2b8-109546296ec5"), 6 },
                    { new Guid("c6d42e3a-6c16-6aaf-30d5-6b22ecbd059f"), 4 },
                    { new Guid("c6f4df9e-50be-e3a9-f9cc-18d85b8cc08d"), 8 },
                    { new Guid("c937f984-8a31-52f5-0542-3fbd7b797e4d"), 6 },
                    { new Guid("d0637781-ed9e-6e85-e7f2-341b9d7c15df"), 7 },
                    { new Guid("d0c020a5-7020-a056-c4a9-3e6e3efe410d"), 6 },
                    { new Guid("d3390c66-39bd-8a3a-ca5a-cc5ec4ac57c4"), 9 },
                    { new Guid("d55708e1-68d7-10b3-3ba3-c9ace32324df"), 4 },
                    { new Guid("d5f23e04-4529-1911-56ad-2e575493c7f5"), 1 },
                    { new Guid("d800f17e-280c-cf5b-ce10-2b2a30bf4d85"), 9 },
                    { new Guid("e15b7598-bc58-34cf-f1b9-5e3f433ad8bc"), 10 },
                    { new Guid("e384f8a3-29ee-8d84-c7ce-0164ece0b38f"), 7 },
                    { new Guid("e6211ae8-1247-6824-c70a-db3c7add1596"), 14 },
                    { new Guid("eda3267b-1e3b-415c-96cd-fa2b28ad0884"), 5 },
                    { new Guid("f270bdc7-329f-52f3-61b9-4e5c5ae86531"), 10 },
                    { new Guid("f296158d-ea48-529f-ceff-1f45b47c3fcb"), 1 },
                    { new Guid("f5029edf-f2ad-3c47-1457-fd6bb75b3c01"), 8 },
                    { new Guid("f6e9da8c-7452-e110-2cf9-dc01f4c16453"), 2 },
                    { new Guid("f960fbbf-3abf-486b-4e35-2d5b1f99f0c0"), 10 },
                    { new Guid("fc38325e-639f-c332-202b-764d885ddf91"), 12 },
                    { new Guid("fc56e902-c9b7-a12e-3d7a-8379dce11702"), 1 },
                    { new Guid("fd0eb117-fe83-e95d-9f6a-ef69442b3be8"), 1 },
                    { new Guid("fde654de-90bd-2765-97f0-ac576b9ef897"), 11 },
                    { new Guid("fe75a530-3597-1a70-8e20-0ff66f223075"), 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bar");

            migrationBuilder.DropTable(
                name: "Foo");
        }
    }
}
