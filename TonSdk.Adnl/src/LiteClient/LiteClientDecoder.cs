﻿using System;
using System.Collections.Generic;
using System.Numerics;
using TonSdk.Adnl.TL;
using TonSdk.Core.Crypto;

namespace TonSdk.Adnl.LiteClient
{
    public class LiteClientDecoder
    {
        internal static MasterChainInfo DecodeGetMasterchainInfo(TLReadBuffer buffer)
        {
            // last:tonNode.blockIdExt
            int workchain = buffer.ReadInt32();
            long shard = buffer.ReadInt64();
            int seqno = buffer.ReadInt32();
            BigInteger rootHash = new BigInteger(buffer.ReadInt256());
            BigInteger fileHash = new BigInteger(buffer.ReadInt256());
            
            // state_root_hash:int256
            BigInteger stateRootHash = new BigInteger(buffer.ReadInt256());
            
            // init:tonNode.zeroStateIdExt
            int workchainI = buffer.ReadInt32();
            BigInteger rootHashI = new BigInteger(buffer.ReadInt256());
            BigInteger fileHashI = new BigInteger(buffer.ReadInt256());
            
            BlockIdExternal lastBlock = new BlockIdExternal(workchain, rootHash, fileHash, shard, seqno);
            BlockIdExternal initBlock = new BlockIdExternal(workchainI, rootHashI, fileHashI, 0,0);
            
            return new MasterChainInfo(lastBlock, initBlock, stateRootHash);
        }
        
        internal static MasterChainInfoExternal DecodeGetMasterchainInfoExternal(TLReadBuffer buffer)
        {
            // mode:#
            buffer.ReadUInt32();
            
            // version:int
            int version = buffer.ReadInt32();
            
            // capabilities:long
            long capabilities = buffer.ReadInt64();
            
            // last:tonNode.blockIdExt
            int workchain = buffer.ReadInt32();
            long shard = buffer.ReadInt64();
            int seqno = buffer.ReadInt32();
            BigInteger rootHash = new BigInteger(buffer.ReadInt256());
            BigInteger fileHash = new BigInteger(buffer.ReadInt256());
            
            // last_uTime:int
            int lastUTime = buffer.ReadInt32();
            // now:int
            int time = buffer.ReadInt32();
            // state_root_hash:int256
            BigInteger stateRootHash = new BigInteger(buffer.ReadInt256());
            
            // init:tonNode.zeroStateIdExt
            int workchainI = buffer.ReadInt32();
            BigInteger rootHashI = new BigInteger(buffer.ReadInt256());
            BigInteger fileHashI = new BigInteger(buffer.ReadInt256());
            
            BlockIdExternal lastBlock = new BlockIdExternal(workchain, rootHash, fileHash, shard, seqno);
            BlockIdExternal initBlock = new BlockIdExternal(workchainI, rootHashI, fileHashI, 0,0);

            return new MasterChainInfoExternal(version, capabilities, lastUTime, time, lastBlock, initBlock,
                stateRootHash);
        }
        
        internal static int DecodeGetTime(TLReadBuffer buffer)
        {
            // now:int
            int time = buffer.ReadInt32();
            return time;
        }

        internal static ChainVersion DecodeGetVersion(TLReadBuffer buffer)
        {
            // mode:#
            buffer.ReadInt32();
            // version:int
            int version = buffer.ReadInt32();
            // capabilities:long
            long capabilities = buffer.ReadInt64();
            // now:int
            int time = buffer.ReadInt32();

            return new ChainVersion(version, capabilities, time);
        }

        internal static byte[] DecodeGetBlock(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            byte[] data = buffer.ReadBuffer();
            return data;
        }

        internal static byte[] DecodeBlockHeader(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            // mode:#
            buffer.ReadUInt32();
            
            // header_proof:bytes
            byte[] headerProof = buffer.ReadBuffer();
            return headerProof;
        }

        internal static int DecodeSendMessage(TLReadBuffer buffer) =>  buffer.ReadInt32();

        internal static byte[] DecodeGetAccountState(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            // shardblk:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();

            // shard_proof:bytes
            buffer.ReadBuffer();
            // proof:bytes
            buffer.ReadBuffer();

            return buffer.ReadBuffer();
        }

        internal static RunSmcMethodResult DecodeRunSmcMethod(TLReadBuffer buffer)
        {
            uint mode = buffer.ReadUInt32();
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            // shardblk:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            byte[] shardProof = (mode & (1 << 0)) != 0 ? buffer.ReadBuffer() : null;
            byte[] proof = (mode & (1 << 0)) != 0 ? buffer.ReadBuffer() : null;
            byte[] stateProof = (mode & (1 << 1)) != 0 ? buffer.ReadBuffer() : null;
            byte[] initC7 = (mode & (1 << 3)) != 0 ? buffer.ReadBuffer() : null;
            byte[] libExtras = (mode & (1 << 4)) != 0 ? buffer.ReadBuffer() : null;
            int exitCode = buffer.ReadInt32();
            byte[] result = (mode & (1 << 2)) != 0 ? buffer.ReadBuffer() : null;

            return new RunSmcMethodResult(shardProof, proof, stateProof, initC7, libExtras, exitCode, result);
        }

        internal static ShardInfo DecodeGetShardInfo(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            // shardblk:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            byte[] shardProof = buffer.ReadBuffer();
            byte[] shardDescr = buffer.ReadBuffer();

            return new ShardInfo(shardProof, shardDescr);
        }
        
        internal static byte[] DecodeGetAllShardsInfo(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            buffer.ReadBuffer();
            byte[] data = buffer.ReadBuffer();
            return data;
        }

        internal static byte[] DecodeGetOneTransaction(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();
            
            buffer.ReadBuffer();
            
            byte[] transaction = buffer.ReadBuffer();
            return transaction;
        }

        internal static byte[] DecodeGetTransactions(TLReadBuffer buffer)
        {
            uint count = buffer.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                // id:tonNode.blockIdExt
                buffer.ReadInt32();
                buffer.ReadInt64();
                buffer.ReadInt32();
                buffer.ReadInt256();
                buffer.ReadInt256();
            }

            return buffer.ReadBuffer();
        }
        
        internal static ListBlockTransactionsExternalResult DecodeListBlockTransactionsExternal(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();

            buffer.ReadUInt32();
            bool inComplete = buffer.ReadBool();
            byte[] transactions = buffer.ReadBuffer();
            byte[] proof = buffer.ReadBuffer();

            return new ListBlockTransactionsExternalResult(inComplete, transactions, proof);
        }

        internal static ListBlockTransactionsResult DecodeListBlockTransactions(TLReadBuffer buffer)
        {
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();

            buffer.ReadUInt32();
            bool inComplete = buffer.ReadBool();
            
            uint count = buffer.ReadUInt32();

            List<TransactionId> ids = new List<TransactionId>();
            for (int i = 0; i < count; i++)
            {
                TransactionId id = new TransactionId();
                
                uint mode = buffer.ReadUInt32();
                if ((mode & (1 << 0)) != 0)
                    id.Account = new BigInteger(buffer.ReadInt256());
                if ((mode & (1 << 1)) != 0) 
                    id.Lt = buffer.ReadInt64();
                if ((mode & (1 << 2)) != 0)
                    id.Hash = new BigInteger(buffer.ReadInt256());
                ids.Add(id);
            }

            byte[] proof = buffer.ReadBuffer();

            return new ListBlockTransactionsResult(inComplete, ids.ToArray(), proof);
        }

        internal static ConfigInfo DecodeGetConfigAll(TLReadBuffer buffer)
        {
            // mode:#
            buffer.ReadUInt32();
            
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();

            byte[] stateProof = buffer.ReadBuffer();
            byte[] configProof = buffer.ReadBuffer();

            return new ConfigInfo()
            {
                StateProof = stateProof,
                ConfigProof = configProof
            };
        }

        internal static ValidatorStats DecodeGetValidatorStats(TLReadBuffer buffer)
        {
            // mode:#
            buffer.ReadUInt32();
            
            // id:tonNode.blockIdExt
            buffer.ReadInt32();
            buffer.ReadInt64();
            buffer.ReadInt32();
            buffer.ReadInt256();
            buffer.ReadInt256();

            int count = buffer.ReadInt32();
            bool complete = buffer.ReadBool();
            byte[] stateProof = buffer.ReadBuffer();
            byte[] dataProof = buffer.ReadBuffer();

            return new ValidatorStats()
            {
                Count = count,
                Complete = complete,
                StateProof = stateProof,
                DataProof = dataProof
            };
        }

        internal static LibraryEntry[] DecodeGetLibraries(TLReadBuffer buffer)
        {
            uint count = buffer.ReadUInt32();
            List<LibraryEntry> list = new List<LibraryEntry>();
            for (int i = 0; i < count; i++)
            {
                BigInteger hash = new BigInteger(buffer.ReadInt256());
                byte[] data = buffer.ReadBuffer();
                list.Add(new LibraryEntry()
                {
                    Data = data,
                    Hash = hash
                });
            }

            return list.ToArray();
        }

        internal static ShardBlockProof DecodeGetShardBlockProof(TLReadBuffer buffer)
        {
            // masterchain_id:tonNode.blockIdExt
            int w = buffer.ReadInt32();
            long shard = buffer.ReadInt64();
            int seqno = buffer.ReadInt32();
            BigInteger rootHash = new BigInteger(buffer.ReadInt256());
            BigInteger fileHash = new BigInteger(buffer.ReadInt256());
            BlockIdExternal masterChainId = new BlockIdExternal(w, rootHash, fileHash, shard, seqno);
            
            uint count = buffer.ReadUInt32();

            List<ShardBlockLink> links = new List<ShardBlockLink>();
            for (int i = 0; i < count; i++)
            {
                // from:tonNode.blockIdExt
                int linkFromW = buffer.ReadInt32();
                long linkFromShard = buffer.ReadInt64();
                int linkFromSeqno = buffer.ReadInt32();
                BigInteger linkFromRootHash = new BigInteger(buffer.ReadInt256());
                BigInteger linkFromFileHash = new BigInteger(buffer.ReadInt256());
                BlockIdExternal linkFrom = new BlockIdExternal(linkFromW, linkFromRootHash, linkFromFileHash, linkFromShard, linkFromSeqno);
                
                byte[] proof = buffer.ReadBuffer();
                links.Add(new ShardBlockLink()
                {
                    BlockIdExternal = linkFrom,
                    Proof = proof
                });
            }

            return new ShardBlockProof()
            {
                MasterChainId = masterChainId,
                Links = links.ToArray()
            };
        }
        
        internal static PartialBlockProof DecodeGetBlockProof(TLReadBuffer buffer)
        {
            bool complete = buffer.ReadBool();
            
            // from:tonNode.blockIdExt
            int fromW = buffer.ReadInt32();
            long fromShard = buffer.ReadInt64();
            int fromSeqno = buffer.ReadInt32();
            BigInteger fromRootHash = new BigInteger(buffer.ReadInt256());
            BigInteger fromFileHash = new BigInteger(buffer.ReadInt256());
            BlockIdExternal from = new BlockIdExternal(fromW, fromRootHash, fromFileHash, fromShard, fromSeqno);
            
            // to:tonNode.blockIdExt
            int toW = buffer.ReadInt32();
            long toShard = buffer.ReadInt64();
            int toSeqno = buffer.ReadInt32();
            BigInteger toRootHash = new BigInteger(buffer.ReadInt256());
            BigInteger toFileHash = new BigInteger(buffer.ReadInt256());
            BlockIdExternal to = new BlockIdExternal(toW, toRootHash, toFileHash, toShard, toSeqno);

            List<IBlockLink> blockLinks = new List<IBlockLink>();
            uint count = buffer.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                int kind = buffer.ReadInt32();
                if (kind == -276947985)
                {
                    // liteServer_blockLinkBack
                    bool toKeyBlock = buffer.ReadBool();
                    
                    // from:tonNode.blockIdExt
                    int linkFromW = buffer.ReadInt32();
                    long linkFromShard = buffer.ReadInt64();
                    int linkFromSeqno = buffer.ReadInt32();
                    BigInteger linkFromRootHash = new BigInteger(buffer.ReadInt256());
                    BigInteger linkFromFileHash = new BigInteger(buffer.ReadInt256());
                    BlockIdExternal linkFrom = new BlockIdExternal(linkFromW, linkFromRootHash, linkFromFileHash, linkFromShard, linkFromSeqno);
            
                    // to:tonNode.blockIdExt
                    int linkToW = buffer.ReadInt32();
                    long linkToShard = buffer.ReadInt64();
                    int linkToSeqno = buffer.ReadInt32();
                    BigInteger linkToRootHash = new BigInteger(buffer.ReadInt256());
                    BigInteger linkToFileHash = new BigInteger(buffer.ReadInt256());
                    BlockIdExternal linkTo = new BlockIdExternal(linkToW, linkToRootHash, linkToFileHash, linkToShard, linkToSeqno);

                    byte[] destProof = buffer.ReadBuffer();
                    byte[] proof = buffer.ReadBuffer();
                    byte[] stateProof = buffer.ReadBuffer();
                    
                    blockLinks.Add(new BlockLinkBack()
                    {
                        ToKeyBlock = toKeyBlock,
                        DestProof = destProof,
                        StateProof = stateProof,
                        Proof = proof,
                        From = linkFrom,
                        To = linkTo,
                    });
                }
                if (kind == 1376767516) {
                    
                    // liteServer_blockLinkForward
                    bool toKeyBlock = buffer.ReadBool();
                    
                    // from:tonNode.blockIdExt
                    int linkFromW = buffer.ReadInt32();
                    long linkFromShard = buffer.ReadInt64();
                    int linkFromSeqno = buffer.ReadInt32();
                    BigInteger linkFromRootHash = new BigInteger(buffer.ReadInt256());
                    BigInteger linkFromFileHash = new BigInteger(buffer.ReadInt256());
                    BlockIdExternal linkFrom = new BlockIdExternal(linkFromW, linkFromRootHash, linkFromFileHash, linkFromShard, linkFromSeqno);
            
                    // to:tonNode.blockIdExt
                    int linkToW = buffer.ReadInt32();
                    long linkToShard = buffer.ReadInt64();
                    int linkToSeqno = buffer.ReadInt32();
                    BigInteger linkToRootHash = new BigInteger(buffer.ReadInt256());
                    BigInteger linkToFileHash = new BigInteger(buffer.ReadInt256());
                    BlockIdExternal linkTo = new BlockIdExternal(linkToW, linkToRootHash, linkToFileHash, linkToShard, linkToSeqno);

                    byte[] destProof = buffer.ReadBuffer();
                    byte[] configProof = buffer.ReadBuffer();
                    
                    int validatorSetHash = buffer.ReadInt32();
                    int catchainSeqno = buffer.ReadInt32();

                    List<Signature> signatures = new List<Signature>();
                    uint c = buffer.ReadUInt32();
                    for (int j = 0; j < c; j++)
                    {
                        BigInteger nodeIdShort = new BigInteger(buffer.ReadInt256());
                        byte[] signature = buffer.ReadBuffer();
                        signatures.Add(new Signature()
                        {
                            NodeIdShort = nodeIdShort,
                            SignatureBytes = signature
                        });
                    }
                    
                    blockLinks.Add(new BlockLinkForward()
                    {
                        ToKeyBlock = toKeyBlock,
                        CatchainSeqno = catchainSeqno,
                        ConfigProof = configProof,
                        DestProof = destProof,
                        From = linkFrom,
                        To = linkTo,
                        Signatures = signatures.ToArray(),
                        ValidatorSetHash = validatorSetHash
                    });
                }
            }

            return new PartialBlockProof()
            {
                Complete = complete,
                From = from,
                To = to,
                BlockLinks = blockLinks.ToArray()
            };
        }
    }
}